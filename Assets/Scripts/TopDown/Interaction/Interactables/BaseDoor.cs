using UnityEngine;
using UnityEngine.SceneManagement;
using TopDown.Interaction.SceneLoading;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
public class BaseDoor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Optional: the trigger Collider2D. If null the script will use the Collider2D on this GameObject.")]
    private Collider2D col;

    [SerializeField] [Tooltip("Name of the scene to load (must be added to Build Settings)")]
    private string sceneName = "Basement";

    [SerializeField] [Tooltip("Cooldown (seconds) after Awake and after each use. Set to 0 to disable.")]
    private float doorCooldown = 1.0f;

    private float nextOpenTime;

    [Header("Target Scene")]
    [Tooltip("Optional explicit build index. If >= 0, the door will always load this index and ignore sceneName.")]
    [SerializeField]
    private int sceneBuildIndexOverride = -1;

    [Header("Load/Unload")]
    [SerializeField]
    [Tooltip(
        "Delay (seconds) before unloading the previous scene after the new scene is loaded. Set to 0 for immediate unload.")]
    private float unloadDelay = 0.1f;

    private void Awake()
    {
        // Use Time.time (seconds since startup) for cooldown checks.
        nextOpenTime = Time.time + doorCooldown;
        Debug.Log($"BasementDoor: cooldown active for {doorCooldown}s, next open at {nextOpenTime:F2}");
    }

    private void Start()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) Debug.LogError("BasementDoor: no Collider2D found on the object.");
        else if (!col.isTrigger) Debug.LogWarning("BasementDoor: Collider2D should be set to Is Trigger.");

        if (string.IsNullOrEmpty(sceneName) && sceneBuildIndexOverride < 0)
            Debug.LogError("BasementDoor: sceneName is empty and no build index override provided.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"BasementDoor: OnTriggerEnter2D with '{other.name}' (tag: {other.tag})");

        // Respect cooldown
        if (Time.time < nextOpenTime)
        {
            Debug.Log($"BasementDoor: Door on cooldown. Next open at {nextOpenTime:F2}, current time {Time.time:F2}");
            return;
        }

        if (!other.CompareTag("Player"))
        {
            Debug.Log("BasementDoor: Ignored - not Player tag.");
            return;
        }

        int buildIndex = -1;
        string targetScene = sceneName;

        if (sceneBuildIndexOverride >= 0)
        {
            buildIndex = sceneBuildIndexOverride;
        }
        else
        {
            // Exact match by scene name to avoid accidental substring collisions.
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                if (!path.EndsWith($"/{sceneName}.unity")) continue;
                buildIndex = SceneUtility.GetBuildIndexByScenePath(path);
                break;
            }
        }

        if (buildIndex < 0 && string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError(
                $"BasementDoor: Scene '{sceneName}' not found in Build Settings and no valid build index override.");
            return;
        }

        Debug.Log($"BasementDoor: Preparing to load scene '{(buildIndex >= 0 ? buildIndex.ToString() : targetScene)}'");

        // Apply cooldown immediately to avoid double-triggering before the scene changes.
        nextOpenTime = Time.time + doorCooldown;

        // Capture the name of the currently active scene so we can unload it later.
        string previousScene = SceneManager.GetActiveScene().name;

        // Start async additive load and swap when ready
        StartCoroutine(LoadAndSwapCoroutine(buildIndex, targetScene, previousScene));
    }

    private IEnumerator LoadAndSwapCoroutine(int buildIndex, string targetScene, string previousScene)
    {
        AsyncOperation ao;
        if (buildIndex >= 0)
        {
            ao = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        }
        else
        {
            ao = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        }

        if (ao == null)
        {
            Debug.LogError("BasementDoor: Failed to start async load.");
            yield break;
        }

        // Wait until load completes
        while (!ao.isDone)
        {
            yield return null;
        }

        // Determine the loaded scene object
        Scene loadedScene;
        if (buildIndex >= 0)
        {
            loadedScene = SceneManager.GetSceneByBuildIndex(buildIndex);
        }
        else
        {
            loadedScene = SceneManager.GetSceneByName(targetScene);
        }

        if (!loadedScene.IsValid())
        {
            Debug.LogError("BasementDoor: Loaded scene is not valid.");
            yield break;
        }

        // Set the newly loaded scene as active so its Awake/Start run in the active context.
        SceneManager.SetActiveScene(loadedScene);

        // If the previous scene is different, schedule its unload after configured delay.
        if (!string.IsNullOrEmpty(previousScene) && previousScene != loadedScene.name)
        {
            CallAfterDelay.Create(Mathf.Max(0f, unloadDelay), () => { SceneHelper.UnloadScene(previousScene); });
        }
    }
}