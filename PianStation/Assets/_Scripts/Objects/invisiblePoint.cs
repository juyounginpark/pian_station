using UnityEngine;

public class PointerVisibilityController : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("포인터 프리팹에 SpriteRenderer 컴포넌트가 필요합니다!", this.gameObject);
        }
    }

    
    void Update()
    {
      
        if (transform.parent != null)
        {
            // 부모의 태그를 매 순간 확인
            if (transform.parent.CompareTag("interact"))
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}