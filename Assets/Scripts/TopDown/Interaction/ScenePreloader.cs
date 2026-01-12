using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDown.Interaction
{
    /// <summary>
    /// ScenePreloader keeps nearby scenes loaded in the background to reduce hitching when
    /// the player moves between adjacent scenes. It loads scenes additively and keeps a small
    /// "preload window" around the current active scene. Scenes outside the window are unloaded.
    ///
    /// How it works:
    /// - The component is a DontDestroyOnLoad singleton so it persists between scene loads.
    /// - When a scene finishes loading (SceneManager.sceneLoaded) it computes the set of
    ///   build indices to keep loaded (current +/- preloadDistance) and loads/unloads accordingly.
    /// - Loading is done with LoadSceneAsync in Additive mode so the scene is resident but not
    ///   made active (SetActiveScene is left to Unity when you explicitly change scenes).
    ///
    /// Notes:
    /// - Make sure the scenes you want preloaded are present in File > Build Settings... in the
    ///   correct order (build indices are used).
    /// - Keep preloadDistance small (1 or 2) to avoid excessive memory use.
    /// </summary>
    // [DefaultExecutionOrder(-100)] // run early
    public class ScenePreloader : MonoBehaviour
    {
        public static ScenePreloader Instance { get; private set; }

        [Tooltip("How many scenes away from the current scene to preload (1 = immediate neighbors).")] [SerializeField]
        private int preloadDistance = 1;

        [Tooltip("Enable debugging logs for preload operations.")] [SerializeField]
        private bool enableDebug = true;

        [Tooltip(
            "If enabled, logs loading progress every frame (very spammy). Keep OFF unless debugging a specific issue.")]
        [SerializeField]
        private bool logLoadingProgressEachFrame;

        [Header("Transition")]
        [Tooltip(
            "If set, this object (and its hierarchy) will be moved into the newly loaded scene during transitions. If null, we search by playerTag.")]
        [SerializeField]
        private GameObject playerRoot;

        [Tooltip("Tag used to find the player root if playerRoot is not assigned.")] [SerializeField]
        private string playerTag = "Player";

        [Tooltip("Prevents starting a new transition while another is in progress.")] [SerializeField]
        private bool preventOverlappingTransitions = true;

        // Tracks build indices we have preloaded (additively loaded).
        private HashSet<int> preloadedIndices = new HashSet<int>();

        // Guards to prevent starting the same load/unload multiple times.
        private HashSet<int> loadingIndices = new HashSet<int>();
        private HashSet<int> unloadingIndices = new HashSet<int>();

        private bool isTransitioning;

        // Preload operations (scenes loaded to 90% with allowSceneActivation=false).
        private Dictionary<int, AsyncOperation> preloadOps = new Dictionary<int, AsyncOperation>();

        private void Awake()
        {
            // singleton pattern so the preloader persists between scene loads
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Ensure an instance exists even if the user didn't add the component to a GameObject.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureExists()
        {
            if (Instance != null) return;
            var go = new GameObject("ScenePreloader");
            go.AddComponent<ScenePreloader>();
            DontDestroyOnLoad(go);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            // handle the current active scene at startup
            var active = SceneManager.GetActiveScene();
            if (active.IsValid())
            {
                if (enableDebug)
                    Debug.Log($"ScenePreloader: Start - active scene '{active.name}' ({active.buildIndex})");
                OnSceneLoaded(active, LoadSceneMode.Single);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (enableDebug) Debug.Log($"ScenePreloader: OnSceneLoaded '{scene.name}' (buildIndex {scene.buildIndex})");

            // IMPORTANT: During a transition we can load scenes additively which triggers this callback.
            // If we react here while the old scene is still the active scene, UpdatePreloadedScenes may
            // compute the wrong window and start unloading the freshly-loaded target.
            if (isTransitioning)
            {
                if (enableDebug) Debug.Log("ScenePreloader: Ignoring sceneLoaded during transition.");
                return;
            }

            // When scenes are loaded additively we can get multiple callbacks close together.
            // Always base our window on the *current* active scene.
            var active = SceneManager.GetActiveScene();
            int activeIndex = active.IsValid() ? active.buildIndex : scene.buildIndex;
            UpdatePreloadedScenes(activeIndex);
        }

        /// <summary>
        /// Public API: Load a scene by build index using additive loading then switch to it and
        /// unload the previous (non-persistent) scenes. This makes use of additive preloading
        /// and avoids the default single-mode unload hitch.
        /// </summary>
        public void LoadSceneAdditiveAndUnloadCurrent(int buildIndex)
        {
            if (preventOverlappingTransitions && isTransitioning)
            {
                if (enableDebug)
                    Debug.LogWarning("ScenePreloader: Transition requested while already transitioning. Ignored.");
                return;
            }

            StartCoroutine(LoadAndSwitchCoroutine(buildIndex));
        }

        /// <summary>
        /// Stable scene transition by scene name. This resolves the scene name to a build index
        /// at runtime (Build Settings), then uses the additive transition path.
        /// </summary>
        public void LoadSceneAdditiveAndUnloadCurrent(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                if (enableDebug) Debug.LogWarning("ScenePreloader: LoadSceneAdditiveAndUnloadCurrent called with empty sceneName.");
                return;
            }

            int buildIndex = -1;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                if (path.EndsWith($"/{sceneName}.unity"))
                {
                    buildIndex = SceneUtility.GetBuildIndexByScenePath(path);
                    break;
                }
            }

            if (buildIndex < 0)
            {
                Debug.LogError($"ScenePreloader: Scene '{sceneName}' not found in Build Settings.");
                return;
            }

            if (enableDebug) Debug.Log($"ScenePreloader: Resolved scene '{sceneName}' to buildIndex {buildIndex}");
            LoadSceneAdditiveAndUnloadCurrent(buildIndex);
        }

        private System.Collections.IEnumerator LoadAndSwitchCoroutine(int buildIndex)
        {
            isTransitioning = true;

            if (enableDebug) Debug.Log($"ScenePreloader: LoadAndSwitch {buildIndex}");

            if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                if (enableDebug) Debug.LogWarning($"ScenePreloader: buildIndex {buildIndex} out of range.");
                isTransitioning = false;
                yield break;
            }

            // Protect the target index from being considered unloadable while transitioning.
            preloadedIndices.Add(buildIndex);

            // Remember previous active scene so we only unload that one (and not our preloads).
            var previousActive = SceneManager.GetActiveScene();

            // If the scene was preloaded with allowSceneActivation=false, activate it now.
            if (preloadOps.TryGetValue(buildIndex, out var preloadOp) && preloadOp != null)
            {
                if (enableDebug) Debug.Log($"ScenePreloader: Activating preloaded scene {buildIndex}...");
                preloadOp.allowSceneActivation = true;

                // Wait for completion (it will finish from ~0.9 to 1.0).
                while (!preloadOp.isDone) yield return null;
                preloadOps.Remove(buildIndex);
            }

            // 1) Ensure target scene is loaded additively.
            var targetScene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (!targetScene.isLoaded)
            {
                if (!loadingIndices.Add(buildIndex))
                {
                    // Someone else is already loading it.
                    while (!SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded) yield return null;
                }
                else
                {
                    var op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                    if (op == null)
                    {
                        loadingIndices.Remove(buildIndex);
                        if (enableDebug) Debug.LogWarning($"ScenePreloader: Failed to start load for {buildIndex}");
                        isTransitioning = false;
                        yield break;
                    }

                    while (!op.isDone) yield return null;
                    loadingIndices.Remove(buildIndex);

                    preloadedIndices.Add(buildIndex);
                }
            }

            targetScene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (!targetScene.IsValid() || !targetScene.isLoaded)
            {
                if (enableDebug)
                    Debug.LogWarning($"ScenePreloader: Target scene {buildIndex} not valid/loaded after load.");
                isTransitioning = false;
                yield break;
            }

            // 2) Move the player into the target scene BEFORE unloading the previous scene.
            var player = playerRoot;
            if (player == null)
            {
                var found = GameObject.FindGameObjectWithTag(playerTag);
                player = found;
            }

            if (player != null)
            {
                SceneManager.MoveGameObjectToScene(player, targetScene);
                if (enableDebug) Debug.Log($"ScenePreloader: Moved player '{player.name}' to scene '{targetScene.name}'.");
            }
            else
            {
                if (enableDebug)
                    Debug.LogWarning(
                        "ScenePreloader: Could not find player to move (playerRoot not set and tag not found). Player may remain in previous scene.");
            }

            // 3) Set target scene active.
            SceneManager.SetActiveScene(targetScene);

            // 4) Unload ONLY the previous active scene (not preloaded neighbors).
            if (previousActive.IsValid() && previousActive.isLoaded && previousActive.buildIndex != targetScene.buildIndex)
            {
                // Avoid unloading if previous active is not in build settings (buildIndex can be -1)
                if (previousActive.buildIndex >= 0)
                {
                    yield return UnloadAsync(previousActive.buildIndex);
                }
                else
                {
                    if (enableDebug)
                        Debug.LogWarning(
                            $"ScenePreloader: Previous active scene '{previousActive.name}' has buildIndex -1, skipping unload.");
                }
            }

            // 5) Update preload window around new active.
            UpdatePreloadedScenes(targetScene.buildIndex);

            isTransitioning = false;
        }

        private void UpdatePreloadedScenes(int currentIndex)
        {
            int total = SceneManager.sceneCountInBuildSettings;
            var desired = new HashSet<int>();

            for (int d = 1; d <= Mathf.Max(0, preloadDistance); d++)
            {
                int left = currentIndex - d;
                int right = currentIndex + d;
                if (left >= 0) desired.Add(left);
                if (right < total) desired.Add(right);
            }

            // Start preloading desired scenes (load to 90% and hold with allowSceneActivation=false).
            foreach (var idx in desired)
            {
                if (idx == currentIndex) continue;
                if (loadingIndices.Contains(idx)) continue;

                var s = SceneManager.GetSceneByBuildIndex(idx);
                if (s.isLoaded)
                {
                    preloadedIndices.Add(idx);
                    continue;
                }

                StartCoroutine(PreloadSceneAsync(idx));
            }

            // Unload scenes we preloaded that are no longer desired. Never unload the active scene.
            var active = SceneManager.GetActiveScene();
            int activeIndex = active.IsValid() ? active.buildIndex : currentIndex;

            var toRemove = new List<int>();
            foreach (var idx in preloadedIndices)
            {
                if (idx == activeIndex) continue;
                if (desired.Contains(idx)) continue;
                if (unloadingIndices.Contains(idx)) continue;

                // If we have a deferred preload op, cancel it by allowing activation and then unloading.
                // (Unity doesn't support true cancellation.)
                if (preloadOps.TryGetValue(idx, out var op) && op != null)
                {
                    op.allowSceneActivation = true;
                    preloadOps.Remove(idx);
                }

                StartCoroutine(UnloadAsync(idx));
                toRemove.Add(idx);
            }

            foreach (var r in toRemove) preloadedIndices.Remove(r);
        }

        /// <summary>
        /// Preload a scene additively but keep it inactive by setting allowSceneActivation=false.
        /// The async op will progress to ~0.9 and then wait.
        /// </summary>
        private System.Collections.IEnumerator PreloadSceneAsync(int buildIndex)
        {
            if (enableDebug) Debug.Log($"ScenePreloader: Preloading scene index {buildIndex}...");

            // Sanity check
            if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                if (enableDebug) Debug.LogWarning($"ScenePreloader: buildIndex {buildIndex} out of range.");
                yield break;
            }

            // Prevent duplicate loads
            if (!loadingIndices.Add(buildIndex)) yield break;

            if (preloadOps.ContainsKey(buildIndex))
            {
                loadingIndices.Remove(buildIndex);
                yield break;
            }

            var alreadyLoaded = SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;
            if (alreadyLoaded)
            {
                loadingIndices.Remove(buildIndex);
                preloadedIndices.Add(buildIndex);
                yield break;
            }

            var op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            if (op == null)
            {
                loadingIndices.Remove(buildIndex);
                if (enableDebug)
                    Debug.LogWarning($"ScenePreloader: Failed to start async load for index {buildIndex}.");
                yield break;
            }

            op.allowSceneActivation = false;
            preloadOps[buildIndex] = op;

            // Wait until the operation hits the preload barrier (~0.9)
            while (!op.isDone)
            {
                if (enableDebug && logLoadingProgressEachFrame)
                    Debug.Log($"ScenePreloader: loading {buildIndex} progress {op.progress:F2}");

                // When allowSceneActivation is false, progress will stick at ~0.9.
                if (op.progress >= 0.9f) break;
                yield return null;
            }

            loadingIndices.Remove(buildIndex);
            preloadedIndices.Add(buildIndex);
            if (enableDebug) Debug.Log($"ScenePreloader: Preloaded scene index {buildIndex} (activation deferred).");
        }

        private System.Collections.IEnumerator UnloadAsync(int buildIndex)
        {
            if (enableDebug) Debug.Log($"ScenePreloader: Unloading preloaded scene index {buildIndex}...");

            if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                if (enableDebug) Debug.LogWarning($"ScenePreloader: buildIndex {buildIndex} out of range for unload.");
                yield break;
            }

            // Never unload the active scene.
            var active = SceneManager.GetActiveScene();
            if (active.IsValid() && active.buildIndex == buildIndex)
            {
                if (enableDebug) Debug.Log($"ScenePreloader: Skipping unload of active scene {buildIndex}.");
                yield break;
            }

            // Prevent duplicate unloads
            if (!unloadingIndices.Add(buildIndex)) yield break;

            // If it's already not loaded, just clean up tracking.
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                unloadingIndices.Remove(buildIndex);
                yield break;
            }

            var op = SceneManager.UnloadSceneAsync(buildIndex);
            if (op == null)
            {
                unloadingIndices.Remove(buildIndex);
                if (enableDebug) Debug.LogWarning($"ScenePreloader: Failed to start unload for index {buildIndex}.");
                yield break;
            }

            while (!op.isDone) yield return null;

            unloadingIndices.Remove(buildIndex);
            if (enableDebug) Debug.Log($"ScenePreloader: Unloaded scene index {buildIndex}.");
        }
    }
}