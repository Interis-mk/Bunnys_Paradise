using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDown.Interaction.Interactables
{
    /// <summary>
    /// Simple door behaviour that loads a target scene when the player enters the trigger.
    /// The door uses a cooldown to prevent immediate re-use (useful to avoid multiple
    /// trigger events firing before a scene change completes).
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class BaseDoor : MonoBehaviour
    {
        ///<summary>
        /// Optional reference to the Collider2D that should be marked as "Is Trigger".
        /// If left empty the component on the same GameObject will be used.
        /// </summary>
        [SerializeField]
        [Tooltip("Optional: the trigger Collider2D. If null the script will use the Collider2D on this GameObject.")]
        private Collider2D col;

        /// <summary>
        /// Name of the scene to load when the player enters the trigger. The scene must be
        /// added to File > Build Settings... for SceneManager.LoadScene to work with a build index.
        /// </summary>
        [SerializeField] [Tooltip("Name of the scene to load (must be added to Build Settings)")]
        private string sceneName = "Basement";

        /// <summary>
        /// Cooldown in seconds applied after Awake and after each door use. While the cooldown
        /// is active the door will ignore trigger events.
        /// Set this to 0 to disable the cooldown.
        /// </summary>
        [SerializeField] [Tooltip("Cooldown (seconds) after Awake and after each use. Set to 0 to disable.")]
        private float doorCooldown = 1.0f;

        /// <summary>
        /// Internal timestamp (Time.time) indicating when the door will next accept a trigger.
        /// </summary>
        private float nextOpenTime;

        /// <summary>
        /// Optional explicit build index. If >= 0, the door will always load this index and ignore sceneName.
        /// </summary>
        [Header("Target Scene")]
        [Tooltip("Optional explicit build index. If >= 0, the door will always load this index and ignore sceneName.")]
        [SerializeField]
        private int sceneBuildIndexOverride = 1;

        /// <summary>
        /// Initialize the door's cooldown so it cannot be used immediately on scene start.
        /// Uses Time.time so the cooldown is independent of frame rate.
        /// </summary>
        private void Awake()
        {
            nextOpenTime = Time.time + doorCooldown;
            Debug.Log($"BasementDoor: cooldown active for {doorCooldown}s, next open at {nextOpenTime:F2}");
        }

        /// <summary>
        /// Cached component checks and helpful setup warnings.
        /// This runs after Awake in normal Unity lifecycle order.
        /// </summary>
        private void Start()
        {
            if (col == null) col = GetComponent<Collider2D>();
            if (col == null) Debug.LogError("BasementDoor: no Collider2D found on the object.");
            else if (!col.isTrigger) Debug.LogWarning("BasementDoor: Collider2D should be set to Is Trigger.");

            if (string.IsNullOrEmpty(sceneName)) Debug.LogError("BasementDoor: sceneName is empty.");
        }

        /// <summary>
        /// Handles 2D trigger enter events. If the entering object has the "Player" tag
        /// and the door is not on cooldown, the target scene will be loaded.
        /// </summary>
        /// <param name="other">The Collider2D that entered this trigger.</param>
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

            int buildIndex;
            if (sceneBuildIndexOverride >= 0)
            {
                buildIndex = sceneBuildIndexOverride;
            }
            else
            {
                // Exact match by scene name to avoid accidental substring collisions.
                buildIndex = -1;
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    string path = SceneUtility.GetScenePathByBuildIndex(i);

                    // GetBuildIndexByScenePath returns the correct build index for the given path.
                    // We only accept an exact filename match ("SceneName.unity").
                    if (!path.EndsWith($"/{sceneName}.unity")) continue;

                    buildIndex = SceneUtility.GetBuildIndexByScenePath(path);
                    break;
                }
            }

            if (buildIndex < 0)
            {
                Debug.LogError(
                    $"BasementDoor: Scene '{sceneName}' not found in Build Settings. Add it via File > Build Settings...");
                return;
            }

            Debug.Log($"BasementDoor: Loading scene '{sceneName}' (resolved build index {buildIndex})");

            // Apply cooldown immediately to avoid double-triggering before the scene changes.
            nextOpenTime = Time.time + doorCooldown;

            // Prefer using the preloader manager if present to reduce hitching.
            if (TopDown.Interaction.ScenePreloader.Instance != null)
            {
                // Use name-based transition so Build Settings reorder/overrides don't accidentally target the wrong scene.
                TopDown.Interaction.ScenePreloader.Instance.LoadSceneAdditiveAndUnloadCurrent(sceneName);
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }
    }
}