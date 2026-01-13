using UnityEngine;

public class SceneSpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnPointID;
    public string SpawnPointID => spawnPointID;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}