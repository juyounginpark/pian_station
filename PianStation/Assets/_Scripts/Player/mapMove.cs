using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    [Tooltip("패럴랙스 효과의 강도. 0이면 움직이지 않고, 1이면 카메라와 똑같이 움직입니다.")]
    [SerializeField] private float parallaxFactor;

    void Start()
    {
        // Main Camera를 찾아서 참조합니다.
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // 카메라가 움직인 거리 계산
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        
        // 배경 오브젝트를 패럴랙스 계수에 따라 이동
        transform.position += new Vector3(deltaX * parallaxFactor, 0, 0);

        // 다음 프레임을 위해 현재 카메라 위치 저장
        lastCameraPosition = cameraTransform.position;
    }
}