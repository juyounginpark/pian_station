//예시1
using UnityEngine;
public class Poster : MonoBehaviour, IInteractable
{
    public GameObject PosterDialogue;
    public void Interact()
    {
        PosterDialogue.gameObject.SetActive(true);
    }
}