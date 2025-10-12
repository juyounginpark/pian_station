using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeMagnitude = 0.1f; // 흔들림 강도
    [SerializeField] private float shakeSpeed = 10f;      // 흔들림 속도
    [SerializeField] private bool isShaking = true;      // 흔들림 활성화 여부

    private Vector3 originalPosition; // 카메라의 원래 위치
    private Coroutine shakeCoroutine; // 코루틴 참조

    void Start()
    {
        // 카메라의 초기 위치 저장
        originalPosition = transform.localPosition;
        shakeCoroutine = StartCoroutine(Shake());
    }

    void Update()
    {
        // 디버깅용: 키보드 입력으로 흔들림 토글 (예: Space 키)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleShake();
        }
    }

    // 흔들림 토글 함수
    public void ToggleShake()
    {
        isShaking = !isShaking;

        if (isShaking && shakeCoroutine == null)
        {
            // 흔들림 시작
            shakeCoroutine = StartCoroutine(Shake());
        }
        else if (!isShaking && shakeCoroutine != null)
        {
            // 흔들림 중지
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            transform.localPosition = originalPosition; // 원래 위치로 복구
        }
    }

    // 흔들림을 처리하는 코루틴
    private IEnumerator Shake()
    {
        while (isShaking)
        {
            // Perlin Noise를 사용해 자연스러운 흔들림 생성
            float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f * shakeMagnitude;
            float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f * shakeMagnitude;

            // 카메라 위치 업데이트
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            // 다음 프레임까지 대기
            yield return null;
        }
    }

    // 외부에서 흔들림 상태를 설정하는 함수
    public void SetShake(bool shake)
    {
        if (shake != isShaking)
        {
            ToggleShake();
        }
    }
}