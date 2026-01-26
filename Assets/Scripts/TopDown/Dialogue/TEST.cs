using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
	[SerializeField] private DialogueContainer test;
	[SerializeField] private string textToTestKey;

	private void Start()
	{
		test.Load();
		Debug.Log(test.AllDialogue[0].DialogueItems[0]);
		DialogueContainer.instance.QueueDialogue(textToTestKey);
	}

	private void FixedUpdate()
	{
		Debug.Log(test.AllDialogue[0].DialogueItems[0]);
		test.Load();
	}
}
