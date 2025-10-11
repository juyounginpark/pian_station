using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private Button quitButton; // 종료를 트리거할 버튼

    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(Quit);
        }
        else
        {
            Debug.LogWarning("Quit Button is not assigned!");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#else
        Application.Quit(); // 빌드된 게임 종료
#endif
    }
}