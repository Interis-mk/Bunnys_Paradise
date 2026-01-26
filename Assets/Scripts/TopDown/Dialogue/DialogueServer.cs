using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueServer : MonoBehaviour
{
    bool active = false;
    [SerializeField] TextMeshProUGUI dialogueBox;
    Queue<string> dialogueLines = new Queue<string>();
    ScriptableObject DialogueContainer;
    
    void Start()
    {
        dialogueBox.text = "";
    }
}
