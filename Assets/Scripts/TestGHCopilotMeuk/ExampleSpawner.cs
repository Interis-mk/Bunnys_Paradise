using UnityEngine;
using TMPro;

public class ExampleSpawner : MonoBehaviour
{
    [Tooltip("Optional: assign a pool in the inspector. If left empty the singleton FloatingTextPool.Instance will be used.")]
    [SerializeField] FloatingTextPool pool;
    [Tooltip("If you're spawning UI (TextMeshProUGUI) you likely want to convert world to screen point and set canvas render mode appropriately.")]
    [SerializeField] Camera worldCamera;
    [SerializeField] float spawnLife = 1.2f;

    void Awake()
    {
        // Use the assigned pool if present; otherwise use the singleton
        if (pool == null)
        {
            if (FloatingTextPool.Instance != null)
            {
                pool = FloatingTextPool.Instance;
            }
            else
            {
                Debug.LogWarning("ExampleSpawner: No FloatingTextPool assigned and no singleton found in scene.");
            }
        }
    }

    void Update()
    {
        // Example: spawn floating text where mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            if (pool == null)
            {
                Debug.LogWarning("ExampleSpawner: Unable to spawn because pool is null.");
                return;
            }

            Vector3 spawnWorld = Vector3.zero;
            Ray ray = worldCamera != null ? worldCamera.ScreenPointToRay(Input.mousePosition) : Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                spawnWorld = hit.point;
            }
            else
            {
                // fallback: point 5 units forward
                spawnWorld = ray.GetPoint(5f);
            }

            // velocity upward, random jitter
            Vector3 vel = new Vector3(0f, 1.2f, 0f);
            string text = Random.Range(5, 50).ToString(); // fake damage number
            pool.Spawn(text, Color.yellow, spawnWorld, vel, spawnLife, 0.2f);
        }
    }
}