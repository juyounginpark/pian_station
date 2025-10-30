using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector3 originalVisualScale;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;
    
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
        animator = visualsTransform.GetComponent<Animator>(); 
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (visualsTransform != null)
        {
            originalVisualScale = visualsTransform.localScale;
        }
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
                visualsTransform.localScale = new Vector3(
                    moveInput.x > 0 ? Mathf.Abs(originalVisualScale.x) : -Mathf.Abs(originalVisualScale.x),
                    originalVisualScale.y,
                    originalVisualScale.z
                );
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