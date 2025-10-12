using UnityEngine;
using System.Collections;

public class ShowUIAfterDialogue : MonoBehaviour, IPostDialogueAction
{
    [Header("UI 설정")]
    [Tooltip("대화가 끝난 후 활성화할 UI 게임 오브젝트")]
    [SerializeField] private GameObject uiToShow;

    [Tooltip("UI가 활성화되었을 때 플레이어의 움직임을 막기 위해 Player 오브젝트를 연결하세요.")]
    [SerializeField] private PlayerController playerController;

    [Tooltip("UI가 자동으로 사라지기까지 걸리는 시간(초)")]
    [SerializeField] private float autoCloseDelay = 5f;

    private Coroutine autoCloseCoroutine;

    void Update()
    {
        if (uiToShow != null && uiToShow.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            CloseUI();
        }
    }

    public void OnDialogueEnd()
    {
        // "최초 1회만"을 확인하던 if 문을 제거하여 항상 실행되도록 합니다.
        if (uiToShow != null)
        {
            uiToShow.SetActive(true);
            Debug.Log($"'{uiToShow.name}' UI를 활성화했습니다.");

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            autoCloseCoroutine = StartCoroutine(AutoCloseUIAfterDelay());
        }
    }

    public void CloseUI()
    {
        if (uiToShow != null && uiToShow.activeSelf)
        {
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = null;
            }

            uiToShow.SetActive(false);
            Debug.Log($"'{uiToShow.name}' UI를 비활성화했습니다.");

            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }

    private IEnumerator AutoCloseUIAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        Debug.Log($"{autoCloseDelay}초가 지나 UI를 자동으로 닫습니다.");
        CloseUI();
    }
}