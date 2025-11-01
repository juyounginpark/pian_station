using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class StrangeVendingMachine : MonoBehaviour, IInteractable
{
    public TMP_Text NotificationText;
    public GameObject Dialogue;
    public GameObject ClockImage;
    public bool isGainClock = false;

    public void Interact()
    {
        if (!isGainClock)
        {
            NotificationText.text = "시계를 획득했습니다.";
            ClockImage.SetActive(true);
            NotificationText.gameObject.SetActive(true);
            Invoke("HideNotification", 2f);
            isGainClock = true;
        }
        else
        {
            Dialogue.SetActive(true);
        }
        
    }

    void HideNotification()
    {
        NotificationText.gameObject.SetActive(false);
    }
}