using System;
using UnityEngine;
using TMPro;

public class DialogueOnHover : MonoBehaviour
{
    public string dialogue;
    public GameObject prefab;
    private GameObject instance;

    public void Update()
    {
        if (instance != null)
        {
            instance.GetComponentInChildren<TextMeshProUGUI>().text = dialogue;
        }
    }

    public void StartDialogue()
    {
        instance = Instantiate(prefab, transform.position, Quaternion.identity, gameObject.transform);
    }

    public void StopDialogue()
    {
        DestroyImmediate(instance.gameObject);
        instance = null;
    }
}