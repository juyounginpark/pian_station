using UnityEngine;

public class UIScreenShake : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 originalPos;

    public float shakeDuration = 0f;
    public float shakeMagnitude = 10f; // 픽셀 단위 (10~30 정도 추천)
    private bool isShaking = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            isShaking = true;
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            rect.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            shakeDuration -= Time.deltaTime;
        }
        else if (isShaking)
        {
            rect.anchoredPosition = originalPos;
            isShaking = false;
        }
    }

    // 외부에서 흔들림 실행
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
