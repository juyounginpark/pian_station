using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Player Move Boundaries")]
    [SerializeField] private float minY = -0.3f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 19f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
                transform.localScale = new Vector3(moveInput.x > 0 ? 1f : -1f, 1f, 1f);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
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
        if (interactableLayer.value < 0 || interactableLayer.value >= 32)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        if (hits.Length == 0)
        {
            return;
        }

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}