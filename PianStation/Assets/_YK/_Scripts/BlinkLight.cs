using UnityEngine;
using System.Collections;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private float minBlinkInterval = 0.3f; // 최소 깜빡임 간격 (초)
    [SerializeField] private float maxBlinkInterval = 1f;   // 최대 깜빡임 간격 (초)

    private GameObject[] children;

    void Start()
    {
        // 자식 오브젝트 가져오기
        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }

        // 자식 오브젝트 수 확인
        if (children.Length != 3)
        {
            Debug.LogWarning($"Expected 3 child objects, but found {children.Length}.");
        }

        // 각 자식에 대해 깜빡임 코루틴 시작
        foreach (GameObject child in children)
        {
            StartCoroutine(Blink(child));
        }
    }

    private IEnumerator Blink(GameObject child)
    {
        while (true)
        {
            child.SetActive(false); // 비활성화
            yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
            child.SetActive(true);  // 활성화
            yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
        }
    }
}