using System.Collections;
using TMPro;
using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    public TMP_Text notificationText;
    public GameObject Memo;

    void Start()
    {
        notificationText.gameObject.SetActive(false);
    }
    public void Interact()
    {
        notificationText.text = "보조배터리를 획득했습니다.";
        notificationText.gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay(2f));
        Memo.tag = "interact";
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        notificationText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}