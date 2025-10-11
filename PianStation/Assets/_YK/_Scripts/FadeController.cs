using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeOverlay; // 페이드 오버레이 이미지
    [SerializeField] private float fadeDuration = 2f; // 페이드 인 지속 시간 (초)
    [SerializeField] private bool fadeInOnStart = true; // 시작 시 자동 페이드 인 여부

    void Start()
    {
        // 시작 시 오버레이가 불투명한 상태로 설정
        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 1f;
            fadeOverlay.color = color;
        }

        // 게임 시작 시 페이드 인
        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn());
        }
    }

    // 페이드 인 코루틴
    public IEnumerator FadeIn()
    {
        if (fadeOverlay == null)
        {
            Debug.LogWarning("FadeOverlay is not assigned!");
            yield break;
        }

        float elapsedTime = 0f;
        Color color = fadeOverlay.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeOverlay.color = color;
            yield return null;
        }

        // 페이드 완료 후 알파값을 0으로 고정
        color.a = 0f;
        fadeOverlay.color = color;
        fadeOverlay.gameObject.SetActive(false); // 오버레이 비활성화
    }

    // 외부에서 페이드 인 호출
    public void StartFadeIn()
    {
        fadeOverlay.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }
}