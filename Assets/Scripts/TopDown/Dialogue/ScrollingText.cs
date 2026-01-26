using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TopDown.Dialogue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScrollingText : MonoBehaviour
{
	public static ScrollingText instance;
	[SerializeField] string finalText = "This is a sample scrolling text effect.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
	public TextMeshProUGUI textComponent;
	int currentIndex = 0;
	[SerializeField] float textTypeDelay = 0.5f;
	[SerializeField] float textDisappearanceDelay = 2f;
	
	private Queue<string> textQueue = new Queue<string>();
	private bool isBusy;
	void Start()
	{
		instance = this;
		textComponent.text = "";
	}
	
	private IEnumerator TypeText()
	{
		DialogueContainer.instance.SwitchActive(true);
		isBusy = true;
		while (finalText.Length > currentIndex) 
		{
			textComponent.text = finalText.Substring(0, currentIndex);
			currentIndex++;
			yield return new WaitForSeconds(textTypeDelay); // Delay between each character
		}
		yield return new  WaitForSeconds(textDisappearanceDelay);
		textComponent.text = "";
		currentIndex = 0;
		if(textQueue.Count > 0)
			textQueue.Dequeue();
		DialogueContainer.instance.SwitchActive(false);
		isBusy = false;
	}

	private void Update()
	{
		if (textQueue.Count > 0)
		{
			if (textQueue.Peek() != null)
			{
				if (!isBusy)
				{
					finalText = textQueue.Peek();
					StartCoroutine(TypeText());
				}
			}
		}
	}

	public void MakeTextQueue(string[] textArray)
	{
		textQueue.Clear();
		foreach (string s in textArray)
		{
			textQueue.Enqueue(s);
		}
	}
}