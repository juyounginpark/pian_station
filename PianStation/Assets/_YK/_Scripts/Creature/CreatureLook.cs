using UnityEngine;

public class CreatureLookAtMain : MonoBehaviour
{
    private Transform target;
    private Vector3 originalScale;  // 원래 크기 저장

    void Start()
    {
        GameObject mainObject = GameObject.FindGameObjectWithTag("main");
        if (mainObject != null)
            target = mainObject.transform;
        else
            Debug.LogWarning("Main 태그가 달린 오브젝트를 찾을 수 없습니다!");

        // 원래 크기 저장
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 scale = originalScale;

        if (transform.position.x > target.position.x)
        {
            // 오른쪽에 있을 때 → 왼쪽 바라봄 (기본)
            scale.x = Mathf.Abs(originalScale.x);  // 양수
        }
        else
        {
            // 왼쪽에 있을 때 → 오른쪽 바라봄 (뒤집기)
            scale.x = -Mathf.Abs(originalScale.x); // 음수
        }

        transform.localScale = scale;
    }
}
