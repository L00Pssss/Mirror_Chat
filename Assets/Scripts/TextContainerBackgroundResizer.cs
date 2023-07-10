using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TextContainerBackgroundResizer : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image backgroundImage;

    public float extraWidth = 0f;
    public float extraHeight = 0f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        ResizeBackground();
    }

    private void ResizeBackground()
    {
        if (textComponent == null || backgroundImage == null)
            return;

        Vector2 textSize = textComponent.GetPreferredValues();
        rectTransform.sizeDelta = new Vector2(textSize.x + extraWidth, textSize.y + extraHeight);
        backgroundImage.rectTransform.sizeDelta = rectTransform.sizeDelta;
    }
}