using UnityEngine;
using TMPro;
using System.Collections;

public class Memo : MonoBehaviour, IInteractable
{
    public TMP_Text NotificationText;
    public GameObject MemoImage;
    public GameObject Dialogue;

    public void Interact()
    {
        MemoImage.SetActive(true);
        NotificationText.text = "메모를 획득했습니다";
        NotificationText.gameObject.SetActive(true);

        Invoke("HideNotification", 2f);
        Dialogue.gameObject.SetActive(true);
    }

    void HideNotification()
    {
        NotificationText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}