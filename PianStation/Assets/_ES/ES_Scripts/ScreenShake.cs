using UnityEngine;

public class ScreenShake2D : MonoBehaviour
{
    private Vector3 originalPos;  // 카메라의 원래 위치
    private Camera mainCamera;

    public float shakeDuration = 0f;   // 흔들림 지속 시간
    public float shakeMagnitude = 0.1f; // 흔들림 강도
    public bool isShaking = false;

    void Start()
    {
        mainCamera = Camera.main;
        originalPos = mainCamera.transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            // 2D이므로 X, Y축만 랜덤 이동
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            shakeDuration -= Time.deltaTime;
            isShaking = true;
        }
        else if (isShaking)
        {
            // 흔들림 종료 후 제자리 복귀
            mainCamera.transform.localPosition = originalPos;
            isShaking = false;
        }
    }

    // 외부에서 호출 (흔들림 실행)
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
