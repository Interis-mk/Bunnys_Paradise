using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
	[SerializeField] string finalText = "This is a sample scrolling text effect.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
	[SerializeField] TextMeshProUGUI textComponent;
	int currentIndex = 0;
	void Start()
	{
		textComponent.text = "";
		StartCoroutine(TypeText());
	}
	
	private IEnumerator TypeText()
	{
		while (finalText.Length > currentIndex) 
		{
			/*if (currentIndex > 1000)
			{
				//Logger.Log("Current Index: " + currentIndex);
				Debug.Log("f'ed up while loop");
				throw new Exception("Simulated exception for demonstration purposes.");
			}*/
			
			textComponent.text = finalText.Substring(0, currentIndex);
			currentIndex++;
			yield return new WaitForSeconds(1f); // Delay between each character
		}

		currentIndex = 0; 
	}
}