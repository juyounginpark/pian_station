using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("기차가 출발할 X 좌표")]
    [SerializeField] private float startX = -15f;

    [Tooltip("기차가 도착할 X 좌표")]
    [SerializeField] private float endX = 15f;

    [Tooltip("기차가 화면을 가로지르는 데 걸리는 시간(초)")]
    [SerializeField] private float moveDuration = 5.0f;

    [Header("랜덤 대기 시간 설정")]
    [Tooltip("기차가 출발하기 전 최소 대기 시간")]
    [SerializeField] private float minWaitTime = 3.0f;

    [Tooltip("기차가 출발하기 전 최대 대기 시간")]
    [SerializeField] private float maxWaitTime = 8.0f;

    [Header("애니메이션 효과")]
    [Tooltip("기차가 덜컹거리는 정도")]
    [SerializeField] private float shakeIntensity = 0.1f;

    [Tooltip("가속/감속을 제어하는 커브")]
    [SerializeField] private AnimationCurve accelerationCurve;

    private Vector3 initialPosition; // 기차의 초기 Y, Z 위치를 저장

    void Start()
    {
        // 나중에 덜컹거리는 효과를 줄 때 Y 좌표를 기준으로 삼기 위해 초기 위치를 저장
        initialPosition = transform.position;
        // 게임이 시작되면 바로 기차 운행 루프를 시작
        StartCoroutine(TrainLoopCoroutine());
    }

    // 기차 운행을 무한히 반복하는 메인 코루틴
    private IEnumerator TrainLoopCoroutine()
    {
        // 게임이 실행되는 동안 계속 반복
        while (true)
        {
            // 1. 랜덤 시간 동안 대기
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 2. 기차 이동 코루틴을 실행하고 끝날 때까지 기다림
            yield return StartCoroutine(MoveTrainCoroutine());
        }
    }

    // 실제로 기차를 움직이는 코루틴
    private IEnumerator MoveTrainCoroutine()
    {
        // 기차를 출발 위치로 설정
        transform.position = new Vector3(startX, initialPosition.y, initialPosition.z);

        float elapsedTime = 0f;

        // 설정된 이동 시간이 다 지날 때까지 반복
        while (elapsedTime < moveDuration)
        {
            // 1. 진행률 계산 (0.0 ~ 1.0)
            float progress = elapsedTime / moveDuration;

            // 2. AnimationCurve를 이용해 진행률을 보정하여 가속/감속 효과 생성
            float curveProgress = accelerationCurve.Evaluate(progress);

            // 3. 보정된 진행률을 이용해 현재 X 위치 계산
            float newX = Mathf.Lerp(startX, endX, curveProgress);

            // 4. 덜컹거리는 효과를 위해 Y 위치에 랜덤한 값을 더함
            float randomShake = Random.Range(-shakeIntensity, shakeIntensity);
            float newY = initialPosition.y + randomShake;

            // 5. 계산된 위치로 기차를 이동
            transform.position = new Vector3(newX, newY, initialPosition.z);

            // 경과 시간을 1프레임만큼 증가시키고 다음 프레임까지 대기
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝난 후, 기차를 정확한 도착 위치로 설정 (오차 보정)
        transform.position = new Vector3(endX, initialPosition.y, initialPosition.z);
    }
}