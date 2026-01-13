using System;
using System.Collections;
using System.Collections.Generic;
using TopDown.Dialogue;
using UnityEngine;

public class TEST : MonoBehaviour
{
	[SerializeField] private DialogueContainer test;

	private void Start()
	{
		test.Load();
		Debug.Log(test.AllDialogue[0].DialogueItems[0]);
	}

	private void FixedUpdate()
	{
		Debug.Log(test.AllDialogue[0].DialogueItems[0]);
		test.Load();
	}
}
