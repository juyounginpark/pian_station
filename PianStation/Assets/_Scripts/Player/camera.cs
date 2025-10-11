using UnityEngine;

public class CameraZoneScroll : MonoBehaviour
{
    [SerializeField] private Transform target; // 플레이어 Transform
    [SerializeField] private float playerMoveSpeed;

    [Header("카메라 트리거 영역 (화면 비율)")]
    [Range(0f, 0.5f)]
    [SerializeField] private float leftBoundary = 0.3f; 
    [Range(0.5f, 1f)]
    [SerializeField] private float rightBoundary = 0.7f; 

    [Header("맵 경계 (World 좌표)")]
    [SerializeField] private float minX; // 카메라가 갈 수 있는 가장 왼쪽 X좌표
    [SerializeField] private float maxX; // 카메라가 갈 수 있는 가장 오른쪽 X좌표

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        Vector3 newCameraPosition = transform.position;
        bool cameraMoved = false;

        // 오른쪽 경계를 넘었고, 오른쪽으로 이동 중일 때
        if (viewportPos.x > rightBoundary && horizontalInput > 0)
        {
            newCameraPosition.x += playerMoveSpeed * Time.deltaTime;
            cameraMoved = true;
        }
        // 왼쪽 경계를 넘었고, 왼쪽으로 이동 중일 때
        else if (viewportPos.x < leftBoundary && horizontalInput < 0)
        {
            newCameraPosition.x -= playerMoveSpeed * Time.deltaTime;
            cameraMoved = true;
        }

        // 카메라가 움직였다면, 경계 값을 적용
        if (cameraMoved)
        {
            // 카메라의 제한(Clamp)
            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, minX, maxX);
            transform.position = newCameraPosition;
        }
    }
}