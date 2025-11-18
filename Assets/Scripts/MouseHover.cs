using UnityEngine;

public class MouseHover : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]private LayerMask mouseLayer;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint((Vector2)Input.mousePosition), mainCamera.transform.forward, mouseLayer);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            if (Input.GetMouseButtonDown(0))
            {
                if(hit.collider.gameObject.TryGetComponent<>(out MouseHover mouseHover))
            }
                
        }
    }
}
