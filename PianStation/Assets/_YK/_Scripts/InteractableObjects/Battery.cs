using System.Collections;
using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    public GameObject notificationText;
    public void Interact()
    {
        notificationText.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
