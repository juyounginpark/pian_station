using UnityEngine;
using UnityEngine.UI;

public class OpenAreaButton : MonoBehaviour
{
    [Header("���� ���� ������Ʈ")]
    public GameObject blockedArea;
    public GameObject Dialogue;
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
            Dialogue.SetActive(true);
            Debug.LogWarning("blockedArea�� ������� �ʾҽ��ϴ�!");
            return;
        }

        blockedArea.SetActive(false);
        Debug.Log("��ư Ŭ�� �� ���� ������ ���Ƚ��ϴ�!");

        //gameObject.SetActive(false);
    }
}
