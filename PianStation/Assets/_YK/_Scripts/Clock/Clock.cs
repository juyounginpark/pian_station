using UnityEngine;

public class ClockThrowSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject clockFromVendingUI; // UI 오브젝트
    public GameObject clockPrefab; // 던질 시계 프리팹
    public Transform player; // 플레이어
    public string playerName = "Player"; // 플레이어 오브젝트 이름

    [Header("Throw Settings")]
    public KeyCode equipKey = KeyCode.Alpha3; // 3번 키 (장착)
    public float throwSpeed = 15f; // 던지는 속도
    public float clockLifetime = -1f; // 시계가 사라지는 시간 (-1: 사라지지 않음)

    [Header("Equipped Clock Settings")]
    public Vector3 equippedOffset = new Vector3(0.5f, 0.5f, 0f); // 플레이어로부터 떨어진 위치
    public float followSpeed = 10f; // 플레이어 따라가는 속도

    private bool hasClockItem = false; // 시계 아이템 소유 여부
    private bool isEquipped = false; // 시계 장착 상태
    private GameObject equippedClock; // 장착된 시계 오브젝트
    private Camera mainCamera;

    void Start()
    {
        // 플레이어 자동 찾기
        if (player == null)
        {
            GameObject p = GameObject.Find(playerName);
            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                Debug.LogError($"[ClockThrowSystem] '{playerName}' 플레이어를 찾을 수 없습니다!");
            }
        }

        // 메인 카메라 가져오기
        mainCamera = Camera.main;

        // UI 초기 상태 확인
        UpdateClockItemStatus();
    }

    void Update()
    {
        // UI 상태 체크
        UpdateClockItemStatus();

        // 3번 키 입력 - 장착/해제
        if (Input.GetKeyDown(equipKey) && hasClockItem && player != null)
        {
            if (!isEquipped)
            {
                EquipClock();
            }
            else
            {
                UnequipClock();
            }
        }

        // 장착 중일 때 플레이어 따라다니기
        if (isEquipped && equippedClock != null)
        {
            UpdateEquippedClockPosition();
        }

        // 마우스 클릭 - 던지기
        if (Input.GetMouseButtonDown(0) && isEquipped && equippedClock != null)
        {
            ThrowClock();
        }
    }

    void UpdateClockItemStatus()
    {
        // clockFromVendingUI가 활성화되어 있으면 시계 아이템 보유
        if (clockFromVendingUI != null)
        {
            hasClockItem = clockFromVendingUI.activeSelf;
        }
        else
        {
            hasClockItem = false;
        }
    }

    void EquipClock()
    {
        if (clockPrefab == null)
        {
            Debug.LogError("[ClockThrowSystem] Clock Prefab이 설정되지 않았습니다!");
            return;
        }

        // 시계 오브젝트 생성
        Vector3 spawnPos = player.position + equippedOffset;
        equippedClock = Instantiate(clockPrefab, spawnPos, Quaternion.identity);

        // 시계를 보이게 설정
        equippedClock.SetActive(true);

        // 태그 설정 (Creature가 인식하도록)
        equippedClock.tag = "ThrownClock";

        isEquipped = true;

        Debug.Log("[ClockThrowSystem] 시계 장착!");
    }

    void UnequipClock()
    {
        if (equippedClock != null)
        {
            Destroy(equippedClock);
        }

        isEquipped = false;

        Debug.Log("[ClockThrowSystem] 시계 장착 해제!");
    }

    void UpdateEquippedClockPosition()
    {
        // 플레이어가 보는 방향으로 시계 위치 업데이트
        bool facingRight = player.localScale.x > 0;
        Vector3 targetPos = player.position;
        
        if (facingRight)
        {
            targetPos += new Vector3(equippedOffset.x, equippedOffset.y, equippedOffset.z);
        }
        else
        {
            targetPos += new Vector3(-equippedOffset.x, equippedOffset.y, equippedOffset.z);
        }

        // 부드럽게 따라가기
        equippedClock.transform.position = Vector3.Lerp(
            equippedClock.transform.position, 
            targetPos, 
            followSpeed * Time.deltaTime
        );
    }

    void ThrowClock()
    {
        if (mainCamera == null)
        {
            Debug.LogError("[ClockThrowSystem] Main Camera를 찾을 수 없습니다!");
            return;
        }

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // 시계에 ThrownClock 컴포넌트 추가 (던져진 시계 처리)
        ThrownClock thrownClock = equippedClock.AddComponent<ThrownClock>();
        thrownClock.Initialize(mouseWorldPos, throwSpeed, clockLifetime);

        // 장착 해제
        equippedClock = null;
        isEquipped = false;

        // UI 숨기기 (시계 사용함)
        if (clockFromVendingUI != null)
        {
            clockFromVendingUI.SetActive(false);
        }

        Debug.Log($"[ClockThrowSystem] 시계 던짐! 목표: {mouseWorldPos}");
    }

    // 외부에서 시계 아이템 획득 시 호출
    public void ObtainClockItem()
    {
        if (clockFromVendingUI != null)
        {
            clockFromVendingUI.SetActive(true);
        }
        Debug.Log("[ClockThrowSystem] 시계 아이템 획득!");
    }
}

// 던져진 시계를 처리하는 컴포넌트
public class ThrownClock : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float speed;
    private float lifetime;
    private float timer;
    private bool hasArrived = false;

    public void Initialize(Vector3 target, float spd, float life)
    {
        targetPosition = target;
        startPosition = transform.position;
        speed = spd;
        lifetime = life;
        timer = 0f;
    }

    // Creature가 시계 도착 여부를 확인할 수 있도록
    public bool HasArrived()
    {
        return hasArrived;
    }

    public Vector3 GetFinalPosition()
    {
        return targetPosition;
    }

    void Update()
    {
        if (!hasArrived)
        {
            // 목표 지점으로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 목표 지점 도착 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasArrived = true;
                transform.position = targetPosition;
                Debug.Log("[ThrownClock] 목표 지점 도착!");
            }
        }

        // lifetime이 양수일 때만 시간 체크 (음수면 영구 유지)
        if (hasArrived && lifetime > 0)
        {
            timer += Time.deltaTime;
            if (timer >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌해도 시계는 사라지지 않음
        Debug.Log($"[ThrownClock] {other.gameObject.name}에 닿음!");
        
        // 필요 시 충돌 처리 가능
        // 예: Creature에게 신호 보내기 등
    }
}