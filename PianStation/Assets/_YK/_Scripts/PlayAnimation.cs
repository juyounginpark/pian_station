using UnityEngine;
using UnityEngine.UI;

public class PlayAnimationOnClick : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string triggerName = "Play";

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(PlayAnimation);
        }
    }

    private void PlayAnimation()
    {
        if (targetAnimator.GetBool(triggerName) == false)
        {
            targetAnimator.SetBool(triggerName, true);
        }
        else
        {
            targetAnimator.SetBool(triggerName, false);
        }
    }
}