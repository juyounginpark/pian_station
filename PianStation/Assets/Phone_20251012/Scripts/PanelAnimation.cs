using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = Vector3.one * 0.9f;
        StartCoroutine(ScaleAnimation());
    }

    public void PlayScaleAnimation()
    {
        transform.localScale = Vector3.one * 0.9f;
        StartCoroutine(ScaleAnimation());
    }

    System.Collections.IEnumerator ScaleAnimation()
    {
        float duration = 0.1f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t * t);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}