using UnityEngine;
using System.Collections;

public class BlinkObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // 깜빡일 GameObject
    [SerializeField] private float blinkInterval = 0.5f; // 깜빡임 간격 (초)
    [SerializeField] private bool autoStart = true; // 시작 시 자동 깜빡임 여부

    private Coroutine blinkCoroutine;

    void Start()
    {
        // targetObject가 없으면 이 GameObject를 사용
        if (targetObject == null)
        {
            targetObject = gameObject;
        }

        // 자동 시작 설정
        if (autoStart)
        {
            StartBlinking();
        }
    }

    public void StartBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(Blink());
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        if (targetObject != null)
        {
            targetObject.SetActive(true); // 깜빡임 중지 시 활성화 상태로 복구
        }
    }

    private IEnumerator Blink()
    {
        if (targetObject == null) yield break;

        while (true)
        {
            targetObject.SetActive(false); // 비활성화
            yield return new WaitForSeconds(blinkInterval);
            targetObject.SetActive(true);  // 활성화
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}