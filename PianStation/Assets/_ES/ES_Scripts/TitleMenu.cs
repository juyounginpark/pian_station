using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void StartGame()
    {
        // ��: "MainScene"���� �̵�
        SceneManager.LoadScene("Intro");
    }

    public void ExitGame()
    {
        // �����Ϳ����� ���� �ߴ�, ���忡���� ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
