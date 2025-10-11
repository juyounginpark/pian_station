using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    [Tooltip("패럴랙스 효과의 강도. 0이면 움직이지 않고, 1이면 카메라와 똑같이 움직입니다.")]
    [SerializeField] private float parallaxFactor;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        
        transform.position += new Vector3(deltaX * parallaxFactor, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
}