using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 필요

public class DialogueManager : MonoBehaviour
{
    // 싱글톤(Singleton) 설정: 다른 스크립트에서 쉽게 접근하기 위함
    public static DialogueManager Instance { get; private set; }

    public GameObject dialoguePanel; // 1단계에서 만든 DialoguePanel
    public Text dialogueText;       // 1단계에서 만든 DialogueText

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 오브젝트 이름을 받아와서 그에 맞는 대사를 표시하는 함수
    public void ShowDialogue(string objectName)
    {
        string message = "";

        // switch 문을 사용하여 오브젝트 이름에 따라 다른 대사를 할당
        switch (objectName)
        {
            case "lock":
                message = "굳게 잠겨있다.";
                break;
            case "desk":
                message = "오래된 책상이다. 서랍은 비어있다.";
                break;
            case "potion":
                message = "붉은색 물약이 담겨있다. 마실 수 있을까?";
                break;
            // 필요한 만큼 case를 추가하여 다른 오브젝트에 대한 대사를 설정할 수 있습니다.
            default:
                message = "특별한 것은 없어 보인다."; // 정해진 이름이 아닐 경우 기본 대사
                break;
        }

        dialogueText.text = message;    // Text UI에 대사 적용
        dialoguePanel.SetActive(true);  // 대사창 패널 활성화
    }

    // 대사창을 닫는 함수
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}