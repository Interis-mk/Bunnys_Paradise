using UnityEngine;
using TMPro;

public class DialogueOnHover : MonoBehaviour
{
    public string dialogue;
    public GameObject prefab;
    protected GameObject instance;
    
    private void OnMouseOver()
    { 
        Debug.Log(dialogue);
       instance = Instantiate(prefab, transform.position, Quaternion.identity);
       instance.GetComponentInChildren<TextMeshProUGUI>().text = dialogue;
    }
    private void OnMouseExit()
    {
        Destroy(instance);
        instance = null;
    }
    
}