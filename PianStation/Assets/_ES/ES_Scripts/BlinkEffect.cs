using UnityEngine;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;  // CanvasGroup 연결
    public float blinkSpeed = 3.0f;  // 깜빡이는 속도
    public bool isBlinking = false;  // 자동 깜빡임 여부

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (isBlinking)
        {
            // 노이즈 섞인 알파값
            float noise = Random.Range(-0.1f, 0.1f);
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f) + noise;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            canvasGroup.alpha = alpha;
        }
    }

    // 외부에서 깜빡임 한 번만 실행
    public IEnumerator Flash(float duration)
    {
        isBlinking = true;
        yield return new WaitForSeconds(duration);
        isBlinking = false;
        canvasGroup.alpha = 0f;
    }
}
