using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDown.Interaction.Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseDoor : MonoBehaviour
    {
        [SerializeField] private Collider2D col;
        [SerializeField] private string sceneName = "Basement";
        [SerializeField] private float doorCooldown = 1.0f;
        
        private float nextOpenTime;

        private void Awake()
        {
            nextOpenTime = Time.time + doorCooldown;
            Debug.Log($"BasementDoor: cooldown active for {doorCooldown}s, next open at {nextOpenTime:F2}");
        }

        private void Start()
        {
            if (col == null) col = GetComponent<Collider2D>();
            if (col == null) Debug.LogError("BasementDoor: no Collider2D found on the object.");
            else if (!col.isTrigger) Debug.LogWarning("BasementDoor: Collider2D should be set to Is Trigger.");

            if (string.IsNullOrEmpty(sceneName)) Debug.LogError("BasementDoor: sceneName is empty.");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"BasementDoor: OnTriggerEnter2D with '{other.name}' (tag: {other.tag})");

            // Cooldown check: ignore triggers until nextOpenTime
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
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                if (path.Contains($"/{sceneName}.unity") || path.EndsWith($"/{sceneName}.unity"))
                {
                    buildIndex = i;
                    break;
                }
            }

            if (buildIndex < 0)
            {
                Debug.LogError($"BasementDoor: Scene '{sceneName}' not found in Build Settings. Add it via File > Build Settings...");
                return;
            }

            Debug.Log($"BasementDoor: Loading scene '{sceneName}' (build index {buildIndex})");
            
            nextOpenTime = Time.time + doorCooldown;

            SceneManager.LoadScene(buildIndex);
        }
    }
}