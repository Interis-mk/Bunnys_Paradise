using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Simple UI element that shows the "E" prompt. Attach this to a Canvas prefab.
/// </summary>
public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Image keyImage;
    [SerializeField] private Sprite keySprite;
    
    private void Start()
    {
        if (promptText != null)
        {
            promptText.text = "E";
        }
        
        if (keyImage != null && keySprite != null)
        {
            keyImage.sprite = keySprite;
        }
    }
}