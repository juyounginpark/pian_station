using UnityEngine;

public class TogglePhone : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePhoneAnimation();
        }
    }

    private void TogglePhoneAnimation()
    {
        bool currentState = animator.GetBool("isOpen");
        animator.SetBool("isOpen", !currentState);
    }
}