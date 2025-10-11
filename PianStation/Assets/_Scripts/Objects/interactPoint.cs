using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    [Tooltip("상호작용 포인터로 사용할 프리팹")]
    public GameObject indicatorPrefab;

    [Tooltip("오브젝트 중심에서 포인터가 얼마나 떨어질지 (Y 오프셋)")]
    public float yOffset = 1.5f;

    private GameObject createdIndicator; 

    void Start()
    {
        if (indicatorPrefab != null)
        {
            // 포인터 생성 위치 계산
            Vector3 indicatorPosition = transform.position + new Vector3(0, yOffset, 0);

            // 포인터를 생성하고, 나중에 제어할 수 있도록 변수에 저장
            createdIndicator = Instantiate(indicatorPrefab, indicatorPosition, Quaternion.identity);

            createdIndicator.transform.SetParent(this.transform);
        }
    }

    public void HideIndicator()
    {
        if (createdIndicator != null)
        {
            Destroy(createdIndicator);
        }
    }
}