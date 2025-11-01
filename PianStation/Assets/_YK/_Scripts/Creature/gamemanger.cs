using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Image panelImage;

    [Header("Game Clear UI")]
    public GameObject gameClearPanel;
    public Image clearPanelImage;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;
    public float restartDelay = 3f; // 게임 오버 시에만 사용됨

    private bool isGameOver = false;
    private bool isGameClear = false;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameClearPanel != null)
            gameClearPanel.SetActive(false);

        if (panelImage == null && gameOverPanel != null)
            panelImage = gameOverPanel.GetComponent<Image>();

        if (clearPanelImage == null && gameClearPanel != null)
            clearPanelImage = gameClearPanel.GetComponent<Image>();

        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (isGameOver || isGameClear) return;
        isGameOver = true;
        Debug.Log("[GameManager] Game Over! 페이드 인 시작...");
        StartCoroutine(GameOverSequence());
    }

    public void GameClear()
    {
        if (isGameOver || isGameClear) return;
        isGameClear = true;
        Debug.Log("[GameManager] Game Clear! 페이드 인 시작...");
        StartCoroutine(GameClearSequence());
    }

    IEnumerator GameOverSequence()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (panelImage != null)
        {
            Color c = panelImage.color;
            c.a = 0f;
            panelImage.color = c;
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (panelImage != null)
            {
                Color c = panelImage.color;
                c.a = alpha;
                panelImage.color = c;
            }

            yield return null;
        }

        if (panelImage != null)
        {
            Color c = panelImage.color;
            c.a = 1f;
            panelImage.color = c;
        }

        Debug.Log("[GameManager] 페이드 완료. 3초 후 재시작...");
        yield return new WaitForSeconds(restartDelay);
        RestartGame();
    }

    // --- [수정됨] ---
    IEnumerator GameClearSequence()
    {
        if (gameClearPanel != null)
            gameClearPanel.SetActive(true);

        if (clearPanelImage != null)
        {
            Color c = clearPanelImage.color;
            c.a = 0f;
            clearPanelImage.color = c;
        }

        // 페이드 인
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (clearPanelImage != null)
            {
                Color c = clearPanelImage.color;
                c.a = alpha;
                clearPanelImage.color = c;
            }

            yield return null;
        }

        if (clearPanelImage != null)
        {
            Color c = clearPanelImage.color;
            c.a = 1f;
            clearPanelImage.color = c;
        }

        // [수정] 5초 대기
        Debug.Log("[GameManager] 게임 클리어! 5초 후 UI가 사라집니다...");
        yield return new WaitForSeconds(5f);

        // [수정] UI 끄기 (재시작 대신)
        if (gameClearPanel != null)
            gameClearPanel.SetActive(false);

        // 참고: 게임 클리어 후 몬스터가 다시 플레이어를 쫓지 않도록
        // CreatureAI의 OnTriggerEnter2D에서 몬스터를 비활성화(gameObject.SetActive(false))
        // 하는 로직을 유지하는 것이 좋습니다.
    }
    // --- 수정 끝 ---

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("[GameManager] 씬 재시작!");
    }
}