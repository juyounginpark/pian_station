//예시1
using UnityEngine;
public class Object1 : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("상호작용!");
    }
}