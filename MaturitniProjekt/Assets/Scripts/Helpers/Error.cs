using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Error : MonoBehaviour
{
    private Image backgroundImage;
    private TextMeshProUGUI textMeshPro;

    void Awake()
    {
        backgroundImage = GetComponent<Image>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (backgroundImage != null && textMeshPro != null)
        {
            StartCoroutine(FadeOutAndDestroy(3f));
        }
        else
        {
            Debug.LogError("Error: Missing Image or TextMeshProUGUI component.");
        }
    }

    private IEnumerator FadeOutAndDestroy(float duration)
    {
        float elapsedTime = 0f;
        Color bgColor = backgroundImage.color;
        Color textColor = textMeshPro.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            bgColor.a = alpha;
            textColor.a = alpha;

            backgroundImage.color = bgColor;
            textMeshPro.color = textColor;

            yield return null;
        }

        Destroy(gameObject); // Destroy the GameObject after fading out
    }
}