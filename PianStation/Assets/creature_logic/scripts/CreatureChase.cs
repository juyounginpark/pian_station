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

    private enum State { Idle, Wander, ChasePlayer, ChaseTrain, RushAttack, FakeRushAttack, Hesitating }
    private State currentState = State.Idle;

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
        }

        // --- 경계선(Boundary) 적용 ---
        Vector3 finalPos = transform.position;
        finalPos.x = Mathf.Clamp(finalPos.x, minX, maxX);
        finalPos.y = Mathf.Clamp(finalPos.y, minY, maxY);
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
        }
    }

    void HandleIdle()
    {
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
        if (stateTimer <= 0)
        {
            SetState(State.Idle);
        }
    }

    IEnumerator RushAttackSequence()
    {
        Vector3 rushTarget = player.position;
        float t = 0;
        while (t < 0.4f)
        {
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
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;
        float t = 0f;

        while (t < 0.3f)
        {
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
        if (other.gameObject.name == trainName)
        {
            train = other.transform;
            trainChaseTimer = 3.5f;
            SetState(State.ChaseTrain);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 5초간 추적해야 하므로 Exit에서는 아무것도 하지 않음
    }

    void UpdateTimers()
    {
        if (rushAttackTimer > 0) rushAttackTimer -= Time.deltaTime;
        if (fakeRushTimer > 0) fakeRushTimer -= Time.deltaTime;
        if (stateTimer > 0) stateTimer -= Time.deltaTime;
        // trainChaseTimer는 HandleChaseTrain에서 직접 관리
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

        // 경계 내에서만 wanderTarget 설정 (선택적: homePosition이 경계 밖이면 문제될 수 있음)
        wanderTarget.x = Mathf.Clamp(wanderTarget.x, minX, maxX);
        wanderTarget.y = Mathf.Clamp(wanderTarget.y, minY, maxY);
    }

    void UpdateFacing()
    {
        if (isAttacking && currentState != State.ChaseTrain) return; 
        if (player == null) return;

        Vector3 targetPos = player.position;
        bool faceRight = false;

        if (currentState == State.ChaseTrain && train != null)
        {
            targetPos = train.position;
        }

        if (transform.position.x < targetPos.x)
        {
            faceRight = true;
        }
        else
        {
            faceRight = false;
        }

        if (faceRight)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
}