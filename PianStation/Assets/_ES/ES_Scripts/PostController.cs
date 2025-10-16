using UnityEngine;
using System;
using System.Collections;

public class PostController : MonoBehaviour
{
    [Header("테이프 버튼들 (1~3개 가능)")]
    public TapeButton[] tapeButtons;

    [Header("포스터 떨어지는 연출 설정")]
    public float fallDistance = 1000f;   // Y축으로 얼마나 내려갈지
    public float fallSpeed = 3f;        // 떨어지는 속도

    private int removedTapeCount = 0;
    private bool isFalling = false;

    public event Action<PostController> OnPosterFallen;

    private void Start()
    {
        foreach (var tape in tapeButtons)
        {
            tape.OnTapeRemoved += HandleTapeRemoved;
        }
    }

    private void HandleTapeRemoved()
    {
        removedTapeCount++;
        Debug.Log($"{name}: 테이프 {removedTapeCount}/{tapeButtons.Length} 제거됨");

        if (removedTapeCount >= tapeButtons.Length && !isFalling)
        {
            StartCoroutine(FallDown());
        }
    }

    private IEnumerator FallDown()
    {
        isFalling = true;

        RectTransform rect = GetComponent<RectTransform>();
        Vector2 start = rect.anchoredPosition;
        Vector2 target = start + new Vector2(0, -fallDistance);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fallSpeed;
            rect.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        Debug.Log($"{name}: 포스터 낙하 완료");
        gameObject.SetActive(false);
        OnPosterFallen?.Invoke(this);
    }
}
