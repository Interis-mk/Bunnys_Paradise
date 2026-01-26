using System.Collections;
using System.Collections.Generic;
using TopDown.Dialogue;
using UnityEngine;

public class accesscript : MonoBehaviour
{
    public void Dialogue(string key)
    {
        DialogueContainer.instance.QueueDialogue(key);
    }
}
