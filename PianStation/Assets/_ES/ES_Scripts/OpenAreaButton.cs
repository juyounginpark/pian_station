using UnityEngine;
using UnityEngine.UI;

public class OpenAreaButton : MonoBehaviour
{
    [Header("막힌 구역 오브젝트")]
    public GameObject blockedArea; 
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OpenArea);
    }

    private void OpenArea()
    {
        if (blockedArea == null)
        {
            Debug.LogWarning("blockedArea가 연결되지 않았습니다!");
            return;
        }

        blockedArea.SetActive(false);
        Debug.Log("버튼 클릭 → 막힌 구역이 열렸습니다!");

        //gameObject.SetActive(false);
    }
}
