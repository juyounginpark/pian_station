using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloseDialogue : MonoBehaviour
{
    public GameObject dialoguePanel; // 대화창 패널
    public Button closeButton; // Inspector에서 연결할 버튼

    void Update()
    {
        // 스페이스바 입력 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            closeButton.onClick.Invoke();
            dialoguePanel.SetActive(false);
        }
    }
}