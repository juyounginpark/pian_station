using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CreatureAI : MonoBehaviour
{
    [Header("🎯 Target Settings")]
    public string playerName = "Player";
    public string trainName = "Train";

    [Header("🚶 Idle & Wander Behavior")]
    public float idleWaitTime = 2f;
    public float wanderRange = 5f;
    public float wanderSpeed = 1.2f;

    [Header("🔍 Detection Settings")]
    public float detectionRange = 8f;
    public float loseInterestDistance = 12f;
    public float playerSpeedThreshold = 1.5f;
    public float slowSpeedIgnore = 0.5f; 

    [Header("🏃 Chase Player Settings")]
    public float baseChaseSpeed = 3f;
    public float maxChaseSpeed = 10f;
    public float erraticMovementFactor = 0.8f;

    [Header("🚂 Chase Train Settings")]
    public float trainChaseSpeed = 8f;

    [Header("💥 Attack Pattern 1 - Rush Attack")]
    public float rushAttackDistance = 3f;
    public float rushAttackSpeed = 12f;
    public float rushAttackAnimDuration = 0.5f;
    public Sprite attackSprite;
    public float rushAttackCooldown = 5f;

    [Header("🎭 Attack Pattern 2 - Fake Rush")]
    public float fakeRushDistance = 6f;
    public float fakeRushSpeed = 15f;
    public float fakeRushCooldown = 6f;

    [Header("🗺️ Boundaries")]
    public float minY = -0.5f;
    public float maxY = 3f;
    public float minX = -8f;
    public float maxX = 20.3f;

    private Transform player;
    private Transform train;
    private Vector3 originalScale;
    private Vector3 homePosition;
    private Vector3 wanderTarget;
    private Vector3 lastPlayerPos;

    private float playerSpeed = 0f;
    private float rushAttackTimer = 0f;
    private float fakeRushTimer = 0f;
    private float stateTimer = 0f;
    private float trainChaseTimer = 0f;
    private bool isAttacking = false;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private enum State { Idle, Wander, ChasePlayer, ChaseTrain, RushAttack, FakeRushAttack, Hesitating, ChaseClock }
    private State currentState = State.Idle;
    private Transform clockTarget = null; 

    void Start()
    {
        GameObject p = GameObject.Find(playerName);
        if (p != null)
        {
            player = p.transform;
            lastPlayerPos = player.position;
        }
        else Debug.LogWarning($"[CreatureAI] '{playerName}' 이름을 가진 플레이어를 찾을 수 없습니다.");

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        originalScale = transform.localScale;
        homePosition = transform.position;

        SetState(State.Idle);
    }

    void Update()
    {
        if (player == null) return;

        if (clockTarget == null)
        {
            GameObject clockObj = GameObject.FindGameObjectWithTag("ThrownClock");
            if (clockObj != null)
            {
                clockTarget = clockObj.transform;
                SetState(State.ChaseClock); 
            }
        }

        UpdateTimers();
        UpdatePlayerSpeed(); 
        UpdateFacing(); 

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Wander:
                HandleWander();
                break;
            case State.ChasePlayer:
                HandleChasePlayer();
                break;
            case State.ChaseTrain:
                HandleChaseTrain();
                break;
            case State.RushAttack:
            case State.FakeRushAttack:
                break;
            case State.Hesitating:
                HandleHesitating();
                break;
            case State.ChaseClock:
                HandleChaseClock();
                break;
        }

        // --- [수정] 경계선(Boundary) 적용 ---
        Vector3 finalPos = transform.position; // Handle...()에서 계산된 위치
        finalPos.x = Mathf.Clamp(finalPos.x, minX, maxX); // X는 항상 제한
        
        if (currentState == State.ChaseClock && clockTarget != null)
        {
            ThrownClock thrownClock = clockTarget.GetComponent<ThrownClock>();
            bool isClockDestLow = false;

            if (thrownClock != null)
            {
                // 시계의 *최종 목적지*가 -2 이하인지 확인
                if (thrownClock.GetFinalPosition().y <= -2f) 
                {
                    isClockDestLow = true;
                }
            }
            
            if (isClockDestLow)
            {
                // [핵심 수정]
                // 시계 목적지가 낮으면, Y좌표가 -2.74f보다 내려가지 못하게 '강제'로 고정(Clamp)
                // HandleChaseClock에서 Y를 -2.75로 설정했더라도, 여기서 -2.74로 덮어씁니다.
                finalPos.y = Mathf.Max(finalPos.y, -2.74f);
            }
            else
            {
                // 시계 목적지가 낮지 않으면, 기존 Y 경계선(minY)을 따름
                finalPos.y = Mathf.Clamp(finalPos.y, minY, maxY);
            }
        }
        else
        {
            // 시계를 쫓는 중이 아닐 때, 기존 Y 경계선(minY)을 따름
            finalPos.y = Mathf.Clamp(finalPos.y, minY, maxY);
        }
        
        // 최종 계산된 위치 적용
        transform.position = finalPos;
    }

    void SetState(State newState)
    {
        currentState = newState;
        stateTimer = 0f;
        isAttacking = false; 

        switch (newState)
        {
            case State.Idle:
                stateTimer = idleWaitTime + Random.Range(-0.5f, 0.5f);
                break;
            case State.Wander:
                SetNewWanderTarget();
                break;
            case State.ChasePlayer:
                break;
            case State.ChaseTrain:
                isAttacking = true;
                break;
            case State.RushAttack:
                isAttacking = true;
                rushAttackTimer = rushAttackCooldown;
                StartCoroutine(RushAttackSequence());
                break;
            case State.FakeRushAttack:
                isAttacking = true;
                fakeRushTimer = fakeRushCooldown;
                StartCoroutine(FakeRushSequence());
                break;
            case State.Hesitating:
                stateTimer = Random.Range(0.5f, 1.0f);
                break;
            case State.ChaseClock:
                break;
        }
    }

    void HandleIdle()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange && playerSpeed > playerSpeedThreshold)
        {
            SetState(State.ChasePlayer);
            return;
        }

        if (stateTimer <= 0)
        {
            SetState(State.Wander);
        }
    }

    void HandleWander()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, wanderSpeed * Time.deltaTime);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && playerSpeed > playerSpeedThreshold)
        {
            SetState(State.ChasePlayer);
            return;
        }

        if (Vector3.Distance(transform.position, wanderTarget) < 0.3f)
        {
            SetState(State.Idle);
        }
    }

    void HandleChasePlayer()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > loseInterestDistance)
        {
            SetState(State.Idle);
            return;
        }

        if (playerSpeed < slowSpeedIgnore)
        {
            SetState(State.Idle);
            return;
        }

        if (distance < rushAttackDistance && rushAttackTimer <= 0)
        {
            SetState(State.RushAttack);
            return;
        }
        if (distance < fakeRushDistance && distance > rushAttackDistance && fakeRushTimer <= 0)
        {
            SetState(State.FakeRushAttack);
            return;
        }

        float speedPercent = 1.0f - Mathf.Clamp01(distance / detectionRange);
        float speed = Mathf.Lerp(baseChaseSpeed, maxChaseSpeed, speedPercent);
        speed += Mathf.Sin(Time.time * 5f) * erraticMovementFactor;

        transform.position = Vector3.MoveTowards(transform.position, player.position, Mathf.Abs(speed) * Time.deltaTime);
    }

    void HandleChaseTrain()
    {
        if (trainChaseTimer <= 0 || train == null)
        {
            train = null;
            trainChaseTimer = 0;
            SetState(State.Idle);
            return;
        }

        Vector3 dir = (train.position - transform.position).normalized;
        transform.position += dir * trainChaseSpeed * Time.deltaTime;

        trainChaseTimer -= Time.deltaTime;
    }

    void HandleHesitating()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            return;
        }

        if (stateTimer <= 0)
        {
            SetState(State.Idle);
        }
    }

    // [수정] 순간이동 로직 제거
    void HandleChaseClock()
    {
        if (clockTarget == null)
        {
            SetState(State.Idle);
            return;
        }

        ThrownClock thrownClock = clockTarget.GetComponent<ThrownClock>();
        if (thrownClock == null)
        {
            SetState(State.Idle); // 시계가 파괴되었으면 Idle
            return;
        }

        Vector3 finalClockPos = thrownClock.GetFinalPosition();
        Vector3 targetMovementPos; 

        if (thrownClock.HasArrived())
        {
            targetMovementPos = finalClockPos;
        }
        else
        {
            targetMovementPos = clockTarget.position; // 아직 이동 중이면 현재 위치 추적
        }
        
        // [핵심 수정] 
        // 텔레포트 로직(distanceToFinal < 0.2f)을 제거하고 '항상' MoveTowards를 사용합니다.
        // 이렇게 하면 Update()의 Y좌표 Clamp 로직이 매 프레임 작동할 수 있습니다.
        transform.position = Vector3.MoveTowards(transform.position, targetMovementPos, trainChaseSpeed * Time.deltaTime);
    }

    IEnumerator RushAttackSequence()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            yield break;
        }

        Vector3 rushTarget = player.position;
        float t = 0;
        while (t < 0.4f)
        {
            if (clockTarget != null)
            {
                SetState(State.ChaseClock);
                yield break;
            }

            transform.position = Vector3.MoveTowards(transform.position, rushTarget, rushAttackSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        if (attackSprite != null) spriteRenderer.sprite = attackSprite;
        yield return new WaitForSeconds(rushAttackAnimDuration);
        if (originalSprite != null) spriteRenderer.sprite = originalSprite;

        SetState(State.Hesitating);
    }

    IEnumerator FakeRushSequence()
    {
        if (clockTarget != null)
        {
            SetState(State.ChaseClock);
            yield break;
        }

        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;
        float t = 0f;

        while (t < 0.3f)
        {
            if (clockTarget != null)
            {
                SetState(State.ChaseClock);
                yield break;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, fakeRushSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        transform.position = startPos;
        SetState(State.Hesitating);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1순위: Game Clear (시계 쫓다가 기차에 닿음)
        if (currentState == State.ChaseClock && other.gameObject.name == trainName)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameClear(); 
            }
            else
            {
                Debug.LogError("[CreatureAI] GameManager를 찾을 수 없습니다!");
            }
            
            if (clockTarget != null) Destroy(clockTarget.gameObject);
            
            gameObject.SetActive(false); 
            return; 
        }

        // 2순위: 플레이어와 충돌
        if (other.gameObject.name == playerName)
        {
            if (currentState == State.RushAttack)
            {
                if (playerSpeed < slowSpeedIgnore)
                {
                    Debug.Log("[CreatureAI] 돌진 공격! 하지만 플레이어가 멈춰있어 회피!");
                }
                else
                {
                    Debug.Log("[CreatureAI] 돌진 공격! 움직이는 플레이어 적중!");
                    TriggerGameOver();
                }
            }
            else
            {
                Debug.Log($"[CreatureAI] {currentState} 상태에서 플레이어와 충돌!");
                TriggerGameOver();
            }
            return;
        }

        // 3순위: 기차와 충돌 (시계 추적 중이 아닐 때)
        if (other.gameObject.name == trainName)
        {
            train = other.transform;
            trainChaseTimer = 2f;
            SetState(State.ChaseTrain);
        }
    }

    void TriggerGameOver()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogError("[CreatureAI] GameManager를 찾을 수 없습니다!");
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
    }

    void UpdateTimers()
    {
        if (rushAttackTimer > 0) rushAttackTimer -= Time.deltaTime;
        if (fakeRushTimer > 0) fakeRushTimer -= Time.deltaTime;
        if (stateTimer > 0) stateTimer -= Time.deltaTime;
    }

    void UpdatePlayerSpeed()
    {
        Vector3 delta = player.position - lastPlayerPos;
        playerSpeed = delta.magnitude / Time.deltaTime;
        lastPlayerPos = player.position;
    }

    void SetNewWanderTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRange;
        wanderTarget = homePosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        wanderTarget.x = Mathf.Clamp(wanderTarget.x, minX, maxX);
        wanderTarget.y = Mathf.Clamp(wanderTarget.y, minY, maxY);
    }

    void UpdateFacing()
    {
        if (isAttacking && currentState != State.ChaseTrain) return; 

        Vector3 targetPos = Vector3.zero;
        bool hasTarget = false;

        if (currentState == State.ChaseClock && clockTarget != null)
        {
            targetPos = clockTarget.position;
            hasTarget = true;
        }
        else if (currentState == State.ChaseTrain && train != null)
        {
            targetPos = train.position;
            hasTarget = true;
        }
        else if (player != null)
        {
            targetPos = player.position;
            hasTarget = true;
        }

        if (hasTarget)
        {
            bool faceRight = (transform.position.x < targetPos.x);

            if (faceRight)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }
}