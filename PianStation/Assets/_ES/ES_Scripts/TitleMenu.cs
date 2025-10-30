using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void StartGame()
    {
        // 예: "MainScene"으로 이동
        SceneManager.LoadScene("ES_Puzzle");
    }

    public void ExitGame()
    {
        // 에디터에서는 실행 중단, 빌드에서는 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
