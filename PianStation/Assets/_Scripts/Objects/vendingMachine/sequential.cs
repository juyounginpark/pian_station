using UnityEngine;
using System.Collections;

public class SequentialUIManager : MonoBehaviour, IPostDialogueAction
{
    [Header("UI 연결")]
    [Tooltip("첫 번째 상호작용 시에만 나타나는 UI")]
    [SerializeField] private GameObject firstInteractionUI;

    [Tooltip("2회차부터 반복적으로 나타나는 UI")]
    [SerializeField] private GameObject repeatingInteractionUI;

    [Tooltip("마지막 상호작용 시 나타나는 UI")]
    [SerializeField] private GameObject finalInteractionUI;

    [Tooltip("마지막 UI가 닫힌 후 영구적으로 나타날 UI")]
    [SerializeField] private GameObject permanentUI_afterFinal;

    [Header("설정")]
    [Tooltip("마지막 UI가 나타날 상호작용 횟수")]
    [SerializeField] private int finalInteractionCount = 5;

    [Tooltip("반복 UI가 자동으로 사라지기까지 걸리는 시간(초)")]
    [SerializeField] private float autoCloseDelay = 3f;

    [Tooltip("UI가 활성화되었을 때 플레이어의 움직임을 막기 위해 Player 오브젝트를 연결")]
    [SerializeField] private PlayerController playerController;

    private int currentInteractionCount = 0;
    private Coroutine autoCloseCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (firstInteractionUI != null && firstInteractionUI.activeSelf)
            {
                CloseFirstUI();
            }
            else if (repeatingInteractionUI != null && repeatingInteractionUI.activeSelf)
            {
                CloseRepeatingUI();
            }
            else if (finalInteractionUI != null && finalInteractionUI.activeSelf)
            {
                CloseFinalUI();
            }
        }
    }

    public void OnDialogueEnd()
    {
        currentInteractionCount++;
        if (playerController != null) playerController.enabled = false;

        if (currentInteractionCount == 1)
        {
            if (firstInteractionUI != null) firstInteractionUI.SetActive(true);
        }
        else if (currentInteractionCount == finalInteractionCount)
        {
            if (finalInteractionUI != null) finalInteractionUI.SetActive(true);
        }
        else if (currentInteractionCount < finalInteractionCount)
        {
            if (repeatingInteractionUI != null)
            {
                repeatingInteractionUI.SetActive(true);
                autoCloseCoroutine = StartCoroutine(AutoCloseUIAfterDelay());
            }
        }
        else
        {
             if (playerController != null) playerController.enabled = true;
        }
    }
    
    public void CloseRepeatingUI()
    {
        if (repeatingInteractionUI != null && repeatingInteractionUI.activeSelf)
        {
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = null;
            }
            repeatingInteractionUI.SetActive(false);
            if (playerController != null) playerController.enabled = true;
        }
    }
    
    private IEnumerator AutoCloseUIAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        CloseRepeatingUI();
    }
    
    public void CloseFirstUI()
    {
        if (firstInteractionUI != null && firstInteractionUI.activeSelf)
        {
            firstInteractionUI.SetActive(false);
            if (playerController != null) playerController.enabled = true;
        }
    }

    public void CloseFinalUI()
    {
        if (finalInteractionUI != null && finalInteractionUI.activeSelf)
        {
            finalInteractionUI.SetActive(false);

            if (permanentUI_afterFinal != null)
            {
                permanentUI_afterFinal.SetActive(true);
            }

            if (playerController != null) playerController.enabled = true;
        }
    }
}