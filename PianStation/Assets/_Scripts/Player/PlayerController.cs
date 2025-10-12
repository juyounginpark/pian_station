using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;
    
    // --- 여기가 추가된 부분 ---
    [Header("Visuals")]
    [Tooltip("캐릭터의 스프라이트와 애니메이터가 있는 자식 오브젝트의 Transform")]
    [SerializeField] private Transform visualsTransform;

    [Header("Player Move Boundaries")]
    [SerializeField] private float minY = -0.3f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 19f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 애니메이터는 이제 자식 오브젝트에 있으므로, 자식에서 찾아옵니다.
        animator = visualsTransform.GetComponent<Animator>(); 
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.sqrMagnitude > 0)
        {
            animator.SetBool("isWalking", true);
            if (moveInput.x != 0)
            {
                // --- 여기가 수정된 부분 ---
                // 자기 자신(transform) 대신 visualsTransform을 뒤집습니다.
                visualsTransform.localScale = new Vector3(moveInput.x > 0 ? 1f : -1f, 1f, 1f);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.E) && InteractionManager.instance != null && !InteractionManager.instance.IsDialogueActive)
        {
            Interact();
        }
    }
    
    // ... (FixedUpdate, LateUpdate, Interact 함수는 기존과 동일) ...
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }

    void Interact()
    {
        if (interactableLayer.value == 0) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);
        if (hits.Length > 0)
        {
            Collider2D closestHit = hits[0];
            if (closestHit.CompareTag("interact"))
            {
                string objectName = closestHit.gameObject.name;
                IPostDialogueAction[] postActions = closestHit.GetComponents<IPostDialogueAction>();
                InteractionManager.instance.StartInteraction(objectName, postActions);
            }
        }
    }
}