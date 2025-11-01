using UnityEngine;
using TMPro;
using System.Collections;

public class VendingMachine : MonoBehaviour, IInteractable
{
    public GameObject StrangeObject;
    public GameObject Dialogue;

    public void Interact()
    {
        StrangeObject.SetActive(true);
        gameObject.SetActive(false);
        Dialogue.SetActive(true);
    }
}