using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(this.gameObject.name);
    }
}