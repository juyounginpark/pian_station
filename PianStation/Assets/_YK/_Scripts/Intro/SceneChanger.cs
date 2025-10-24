using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("전환할 씬의 이름")]
    private string targetSceneName = "NextScene"; // 전환할 씬 이름

    public void ChangeScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}