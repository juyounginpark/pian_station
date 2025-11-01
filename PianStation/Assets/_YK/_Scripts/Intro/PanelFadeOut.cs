using UnityEngine;
using UnityEngine.UI;   // Image 사용
using System.Collections;

public class PanelFadeOut : MonoBehaviour
{
    public Image blackPanel;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color color = blackPanel.color;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - (timer / fadeDuration);
            blackPanel.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        blackPanel.gameObject.SetActive(false);
    }
}