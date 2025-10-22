using UnityEngine;

public class DisappearAfterDialogue : MonoBehaviour, IPostDialogueAction
{
    public void OnDialogueEnd()
    {
        gameObject.SetActive(false);
    }
}