using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    // Use TMP_Text so this works with both TextMeshPro and TextMeshProUGUI
    [SerializeField] TMP_Text tmpText;
    [Tooltip("Local velocity in world or canvas space (units per second)")]
    [SerializeField] Vector3 velocity = new Vector3(0, 1f, 0);
    [Tooltip("Lifetime in seconds")]
    [SerializeField] float life = 1.2f;
    [Tooltip("Optional curve to control movement over normalized time (0..1)")]
    [SerializeField] AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [Tooltip("Scale over normalized time (0..1)")]
    [SerializeField] AnimationCurve scaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [Tooltip("Alpha multiplier over normalized time (0..1)")]
    [SerializeField] AnimationCurve alphaCurve = AnimationCurve.Linear(0, 1, 1, 0);

    float elapsed;
    Vector3 startPos;
    Vector3 startScale;
    Vector3 originalVelocity;
    Color originalColor;
    FloatingTextPool pool;

    // Public API to initialize
    public void Init(string text, Color color, Vector3 worldPosition, Vector3 worldVelocity, float lifetime, FloatingTextPool poolOwner, float randomJitter = 0f)
    {
        if (tmpText == null) tmpText = GetComponentInChildren<TMP_Text>();
        pool = poolOwner;
        tmpText.text = text;
        tmpText.color = color;
        originalColor = color;
        transform.position = worldPosition + (Random.insideUnitSphere * randomJitter);
        startPos = transform.position;
        originalVelocity = worldVelocity;
        velocity = worldVelocity;
        life = Mathf.Max(0.01f, lifetime);
        elapsed = 0f;
        startScale = transform.localScale;
        gameObject.SetActive(true);
    }

    void ResetState()
    {
        elapsed = 0f;
        tmpText.alpha = 1f;
        transform.localScale = startScale;
    }

    void OnEnable()
    {
        if (tmpText == null) tmpText = GetComponentInChildren<TMP_Text>();
        originalColor = tmpText != null ? tmpText.color : Color.white;
        ResetState();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / life);

        // Movement: move along velocity modulated by moveCurve
        Vector3 offset = originalVelocity * (moveCurve.Evaluate(t) * life);
        transform.position = startPos + offset;

        // Scale
        float scaleMul = scaleCurve.Evaluate(t);
        transform.localScale = startScale * scaleMul;

        // Fade
        float alpha = alphaCurve.Evaluate(t);
        if (tmpText != null)
        {
            Color c = originalColor;
            c.a = alpha * originalColor.a;
            tmpText.color = c;
        }

        if (elapsed >= life)
        {
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // Deactivate and give back to pool
        gameObject.SetActive(false);
        if (pool != null) pool.Return(this);
        else Destroy(gameObject);
    }
}