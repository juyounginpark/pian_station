//예시1
using UnityEngine;
public class Poster : MonoBehaviour, IInteractable
{
    public GameObject PosterCanvas;
    public void Interact()
    {
        PosterCanvas.gameObject.SetActive(true);
    }
}