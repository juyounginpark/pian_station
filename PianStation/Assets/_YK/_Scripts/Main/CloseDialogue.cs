using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloseDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Button closeButton;

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