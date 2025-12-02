using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextPool : MonoBehaviour
{
    // Singleton instance
    public static FloatingTextPool Instance { get; private set; }

    [Tooltip("Prefab must contain a FloatingText component and a TMP_Text (TextMeshPro or TextMeshProUGUI)")]
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] int initialSize = 10;
    [Tooltip("Parent transform for spawned texts (optional)")]
    [SerializeField] Transform parentTransform;
    [Tooltip("If true, this GameObject won't be destroyed on scene load")]
    [SerializeField] bool dontDestroyOnLoad = false;

    Queue<FloatingText> pool = new Queue<FloatingText>();

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else if (Instance != this)
        {
            Debug.LogWarning("FloatingTextPool: Another instance exists, destroying duplicate.");
            Destroy(this.gameObject);
            return;
        }

        if (parentTransform == null) parentTransform = this.transform;
        for (int i = 0; i < Mathf.Max(0, initialSize); i++)
        {
            // Pre-warm inactive instances into the pool
            CreateNew(active: false);
        }
    }

    // Create a new instance. If active == false we enqueue it into the pool (prewarm).
    // If active == true we create and return it without enqueuing (it's intended to be used immediately).
    FloatingText CreateNew(bool active = false)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingTextPool: floatingTextPrefab is not assigned.");
            return null;
        }

        GameObject go = Instantiate(floatingTextPrefab, parentTransform);
        go.SetActive(active);
        FloatingText ft = go.GetComponent<FloatingText>();
        if (ft == null) ft = go.AddComponent<FloatingText>(); // fallback

        if (!active)
        {
            pool.Enqueue(ft);
        }

        return ft;
    }

    public FloatingText Spawn(string text, Color color, Vector3 worldPos, Vector3 worldVel, float life = 1.2f, float randomJitter = 0f)
    {
        FloatingText ft;
        if (pool.Count > 0)
        {
            ft = pool.Dequeue();
            if (ft == null)
            {
                ft = CreateNew(true);
            }
            else
            {
                ft.gameObject.SetActive(true);
            }
        }
        else
        {
            ft = CreateNew(true);
        }

        ft.Init(text, color, worldPos, worldVel, life, this, randomJitter);
        return ft;
    }

    public void Return(FloatingText ft)
    {
        if (ft == null) return;
        // Ensure it's inactive for reuse
        ft.gameObject.SetActive(false);
        pool.Enqueue(ft);
    }
}