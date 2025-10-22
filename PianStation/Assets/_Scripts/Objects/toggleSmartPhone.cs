using UnityEngine;
using System.Collections;

public class ToggleMoveAnimation : MonoBehaviour
{
    [Header("기준이 될 부모")]
    [Tooltip("이 오브젝트의 움직임 기준이 될 부모(카메라 등)의 Transform을 연결해주세요.")]
    [SerializeField] private Transform parentToFollow;

    [Header("움직임 설정")]
    [Tooltip("기준 위치에서 Y축으로 얼마나 높이 올라갈지")]
    [SerializeField] private float moveDistanceY = 3f;

    [Tooltip("움직임이 완료되기까지 걸리는 시간(초)")]
    [SerializeField] private float moveDuration = 1.0f;

    private Vector3 offsetFromParent;
    private bool isPositionUp = false;
    private bool isMoving = false;

    void Start()
    {
        if (parentToFollow == null)
        {
            Debug.LogError("기준 부모(Parent To Follow)가 설정되지 않았습니다!", this.gameObject);
            this.enabled = false;
            return;
        }
        offsetFromParent = transform.position - parentToFollow.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving)
        {
            Vector3 basePosition = parentToFollow.position + offsetFromParent;
            if (isPositionUp)
            {
                StartCoroutine(MoveObject(basePosition));
            }
            else
            {
                Vector3 targetPosition = basePosition + new Vector3(0, moveDistanceY, 0);
                StartCoroutine(MoveObject(targetPosition));
            }
            isPositionUp = !isPositionUp;
        }
    }

    void LateUpdate()
    {
        if (!isMoving)
        {
            Vector3 currentTargetPosition;
            Vector3 basePosition = parentToFollow.position + offsetFromParent;
            if (isPositionUp)
            {
                currentTargetPosition = basePosition + new Vector3(0, moveDistanceY, 0);
            }
            else
            {
                currentTargetPosition = basePosition;
            }
            transform.position = currentTargetPosition;
        }
    }

    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float newY = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration).y;

            float newX = parentToFollow.position.x + offsetFromParent.x;

            float newZ = parentToFollow.position.z + offsetFromParent.z;

            transform.position = new Vector3(newX, newY, newZ);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(
            parentToFollow.position.x + offsetFromParent.x,
            targetPosition.y,
            parentToFollow.position.z + offsetFromParent.z
        );

        isMoving = false;
    }
}