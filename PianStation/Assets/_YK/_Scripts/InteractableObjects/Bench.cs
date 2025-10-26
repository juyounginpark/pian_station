using System.Collections;
using UnityEngine;

public class Bench : MonoBehaviour, IInteractable
{
    [Header("움직임 설정")]
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveDuration = 1.5f;

    [Header("연결 오브젝트")]
    [Tooltip("움직임이 끝난 후 'interact' 태그를 부여할 오브젝트")]
    [SerializeField] private GameObject targetObjectForTagChange;

    private bool hasActionBeenPerformed = false;
    public GameObject Dialogue;
    private InteractableIndicator indicator;
    public void StartMoveBench()
    {
        StartCoroutine(MoveUpCoroutine());
    }

    public void SetTagTointeract()
    {
        gameObject.tag = "interact";
    }

    public IEnumerator MoveUpCoroutine()
    {
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, moveDistance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        
//        gameObject.layer = LayerMask.NameToLayer("PostInteract");
        
        if (targetObjectForTagChange != null)
        {
            targetObjectForTagChange.tag = "interact";
            Debug.Log($"'{targetObjectForTagChange.name}' 오브젝트의 태그를 'interact'로 변경했습니다.");
        }
    }


    public void Interact()
    {
        Dialogue.SetActive(true);
        gameObject.tag = "Untagged";
    }
}
