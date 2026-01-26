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
    [Header("Spawn Settings")]
    [SerializeField] [Tooltip("The ID of the spawn point in the target scene.")]
    private string targetSpawnID;
    [SerializeField] [Tooltip("Name of the scene to load (must be added to Build Settings)")]
    private string sceneName = "Basement";

    [SerializeField] [Tooltip("Cooldown (seconds) after Awake and after each use. Set to 0 to disable.")]
    private float doorCooldown = 1.0f;

    private float nextOpenTime;
    private GameObject nextScenePlayer;

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

    private IEnumerator LoadAndSwapCoroutine(int buildIndex, string targetSceneName, string previousSceneName)
    {
    // STEP 1: Start the Async Loading process
    AsyncOperation loadOperation;

    if (sceneBuildIndexOverride >= 0)
    {
        loadOperation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
    }
    else
    {
        loadOperation = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
    }

    // Safety check if the load failed to start
    if (loadOperation == null)
    {
        Debug.LogError($"Failed to load scene: {targetSceneName}");
        yield break;
    }

    // STEP 2: Wait for the scene to finish loading into memory
    while (!loadOperation.isDone)
    {
        yield return null;
    }

    // STEP 3: Identify the Scene object that was just loaded
    Scene loadedScene;
    if (sceneBuildIndexOverride >= 0)
    {
        loadedScene = SceneManager.GetSceneByBuildIndex(buildIndex);
    }
    else
    {
        loadedScene = SceneManager.GetSceneByName(targetSceneName);
    }

    // Validate the scene before proceeding
    if (!loadedScene.IsValid())
    {
        Debug.LogError("The loaded scene is not valid. Check Build Settings.");
        yield break;
    }

    // STEP 4: Activate the new scene
    // This ensures new objects are instantiated into the correct scene
    SceneManager.SetActiveScene(loadedScene);

    // STEP 5: Teleport the Player to the correct Spawn Point
    MovePlayerToSpawn(loadedScene);

    // STEP 6: Clean up the old scene
    // We only unload if the scene actually changed
    bool isDifferentScene = !string.IsNullOrEmpty(previousSceneName) && previousSceneName != loadedScene.name;

    if (isDifferentScene)
    {
        // Use the configured delay to allow the camera/player to settle
        float finalDelay = Mathf.Max(0f, unloadDelay);
        
        CallAfterDelay.Create(finalDelay, () => 
        { 
            SceneHelper.UnloadScene(previousSceneName); 
        });
    }
}


private void MovePlayerToSpawn(Scene targetScene)
{
    
    GameObject[] newSceneObjects = targetScene.GetRootGameObjects();
    foreach (GameObject o in newSceneObjects)
    {
        if (o.gameObject.CompareTag("Player"))
        {
            nextScenePlayer = o;
        }
    }
    
    if (nextScenePlayer == null)
    {
        Debug.LogWarning("MovePlayerToSpawn: No object with tag 'Player' found.");
        return;
    }

    // Find all potential spawn points in the game
    SceneSpawnPoint[] allSpawns = Object.FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None);

    foreach (SceneSpawnPoint spawn in allSpawns)
    {
        // Check if this spawn point belongs to the scene we just loaded 
        // AND matches the ID we are looking for
        if (spawn.gameObject.scene == targetScene && spawn.SpawnPointID == targetSpawnID)
        {
            nextScenePlayer.transform.position = spawn.transform.position;
            nextScenePlayer.transform.rotation = spawn.transform.rotation;
            return;
        }
    }

    Debug.LogWarning($"MovePlayerToSpawn: Could not find SpawnPoint ID '{targetSpawnID}' in {targetScene.name}");
}
}
