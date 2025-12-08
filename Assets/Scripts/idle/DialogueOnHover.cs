using System;
using UnityEngine;
using TMPro;

public class DialogueOnHover : MonoBehaviour
{
    public string dialogue;
    public GameObject prefab;
    protected GameObject instance;

    public void Update()
    {
        instance.GetComponentInChildren<TextMeshProUGUI>().text = dialogue;
    }

    public void StartDialogue()
    { 
        Debug.Log(dialogue);
       instance = Instantiate(prefab, transform.position, Quaternion.identity, gameObject.transform);
       
       
    }
    public void StopDialogue()
    {
        Destroy(instance);
        instance = null;
    }
    
}