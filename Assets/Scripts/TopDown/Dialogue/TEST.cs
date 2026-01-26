using System;
using System.Collections;
using System.Collections.Generic;
using TopDown.Dialogue;
using UnityEngine;

public class TEST : MonoBehaviour
{
	[SerializeField] private DialogueContainer test;
	[SerializeField] private string textToTestKey;

	private void Start()
	{
		DialogueContainer.instance.QueueDialogue(textToTestKey);
	}
	
	
}
