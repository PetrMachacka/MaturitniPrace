using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Errors : MonoBehaviour
{
    public static void ShowError(string message)
    {
        // Find the Canvas GameObject
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas GameObject not found in the scene.");
            return;
        }

        // Create the ErrorTextContainer GameObject
        GameObject errorTextContainer = new GameObject("ErrorTextContainer");
        errorTextContainer.transform.SetParent(canvas.transform);

        // Add background image to the container
        Image backgroundImage = errorTextContainer.AddComponent<Image>();
        backgroundImage.color = new Color(0, 0, 0, 0.6f); // semi-transparent black

        // Configure RectTransform for the container
        RectTransform containerRectTransform = errorTextContainer.GetComponent<RectTransform>();
        containerRectTransform.sizeDelta = new Vector2(300, 100); // smaller box
        containerRectTransform.anchorMin = new Vector2(1, 1);
        containerRectTransform.anchorMax = new Vector2(1, 1);
        containerRectTransform.pivot = new Vector2(1, 1);
        containerRectTransform.anchoredPosition = new Vector2(-10, -10); // top right with some margin

        // Create the ErrorText GameObject
        GameObject errorTextObject = new GameObject("ErrorText");
        errorTextObject.transform.SetParent(errorTextContainer.transform);

        // Add TextMeshProUGUI component to the text object
        TextMeshProUGUI textMeshPro = errorTextObject.AddComponent<TextMeshProUGUI>();
        textMeshPro.text = message;
        textMeshPro.fontSize = 18; // smaller font size
        textMeshPro.color = Color.white;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        // Configure RectTransform for the text object
        RectTransform textRectTransform = errorTextObject.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(300, 100); // match the container size
        textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero; // center within the container

        // Add the Error script to the container
        errorTextContainer.AddComponent<Error>();
    }
}