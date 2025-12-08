using UnityEngine;
using TMPro;

public class DialogueOnHover : MonoBehaviour
{
    public string dialogue;
    public GameObject prefab;
    protected GameObject instance;
    
    public void StartDialogue()
    { 
       instance = Instantiate(prefab, transform.position, Quaternion.identity, gameObject.transform);
       instance.GetComponentInChildren<TextMeshProUGUI>().text = dialogue;
       Debug.Log(instance);
       Debug.Log(instance.GetComponentInChildren<TextMeshProUGUI>());
       Debug.Log(instance.GetComponentInChildren<TextMeshProUGUI>().text);
    }
    public void StopDialogue()
    {
        Destroy(instance);
        instance = null;
    }
    
}