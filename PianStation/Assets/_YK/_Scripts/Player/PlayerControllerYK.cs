using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerYK : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;
    private GameObject currentPressE; // 현재 활성화된 PressE 오브젝트

    [Header("Visuals")]
    [Tooltip("캐릭터의 스프라이트와 애니메이터가 있는 자식 오브젝트의 Transform")]
    [SerializeField] private Transform visualsTransform;
    private SpriteRenderer spriteRenderer;

    [Header("Player Move Boundaries")]
    [SerializeField] private float minY = -0.3f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 19f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = visualsTransform.GetComponent<Animator>();
        spriteRenderer = visualsTransform.GetComponent<SpriteRenderer>();
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

    }

    void Update()
    {
        // 이동 입력
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.sqrMagnitude > 0)
        {
            animator.SetBool("isWalking", true);
            if (moveInput.x != 0)
            {
                spriteRenderer.flipX = moveInput.x < 0;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // 상호작용 범위 내 오브젝트 확인
        CheckInteractableProximity();

        // 상호작용 입력 (E 키)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

    }

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

    void CheckInteractableProximity()
    {
        // 현재 PressE 오브젝트 비활성화
        if (currentPressE != null)
        {
            currentPressE.SetActive(false);
            currentPressE = null;
        }

        // interactionRadius 내의 상호작용 가능 오브젝트 감지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);
        foreach (var hit in hits)
        {
            // "Interact" 태그 확인
            if (hit.CompareTag("interact"))
            {
                // 자식 오브젝트에서 PressE 찾기
                Transform pressE = hit.transform.Find("PressE");
                if (pressE != null)
                {
                    currentPressE = pressE.gameObject;
                    currentPressE.SetActive(true); // PressE 오브젝트 활성화
                    break; // 첫 번째 상호작용 오브젝트만 처리
                }
            }
        }
    }

    void TryInteract()
    {
        // interactionRadius 내의 상호작용 가능 오브젝트 감지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);
        foreach (var hit in hits)
        {
            // "Interact" 태그 확인
            if (hit.CompareTag("interact"))
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(); // 상호작용 실행
                    break; // 첫 번째 상호작용 오브젝트만 처리
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 상호작용 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}