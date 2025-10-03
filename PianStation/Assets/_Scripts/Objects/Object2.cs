//예시2
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName = "망치"; 

    public void Interact()
    {
        Debug.Log($"{itemName}을(를) 획득했습니다!");
        // 아이템 획득 로직 추가
        Destroy(gameObject);
    }
}