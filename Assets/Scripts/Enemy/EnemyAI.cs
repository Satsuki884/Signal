using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack, Cooldown }

    [Header("State")]
    public EnemyState currentState = EnemyState.Patrol;

    [Header("Settings")]
    public float aggroRadius = 10f;
    public float attackRadius = 1.5f;
    public float moveSpeed = 3f;

    [Header("Patrol Settings")]
    public float patrolRadius = 5f;
    public float patrolSpeed = 1.5f;
    public float patrolWaitTime = 2f;

    [Header("Attack Timings")]
    public float attackDamageDelay = 0.5f;
    public float postAttackCooldown = 1.5f;

    [Header("Breathing (Idle Sound)")]
    public float breathRadius = 15f;
    public float minBreathTime = 5f;
    public float maxBreathTime = 15f;

    [Header("Components")]
    public Transform player;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    [Header("Audio")]
    public AudioSource breathSource;
    public AudioSource sfxSource;
    public AudioClip breathClip;
    public AudioClip aggroClip;
    public AudioClip attackClip;

    private Coroutine attackRoutine;
    private bool isSonarAggroed;

    private Vector2 startPosition;
    private Vector2 patrolTarget;
    private float patrolWaitTimer;
    private bool isWaiting;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        startPosition = transform.position;
        SetNewPatrolTarget();

        StartCoroutine(BreathingRoutine());
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior(distToPlayer);
                break;
            case EnemyState.Chase:
                ChaseBehavior(distToPlayer);
                break;
            case EnemyState.Attack:
            case EnemyState.Cooldown:
                break;
        }
    }

    private IEnumerator BreathingRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minBreathTime, maxBreathTime);
            yield return new WaitForSeconds(waitTime);

            if (player == null || currentState != EnemyState.Patrol) continue;

            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= breathRadius)
            {
                float volume = 1f - (dist / breathRadius);
                breathSource.volume = Mathf.Clamp(volume, 0f, 1f);

                if (breathClip != null)
                {
                    breathSource.PlayOneShot(breathClip);
                }
            }
        }
    }

    private void PatrolBehavior(float distToPlayer)
    {

        if (distToPlayer <= aggroRadius || isSonarAggroed)
        {
            StartChase();
            return;
        }


        if (isWaiting)
        {
            anim.Play("Idle");
            patrolWaitTimer -= Time.deltaTime;

            if (patrolWaitTimer <= 0)
            {
                isWaiting = false;
                SetNewPatrolTarget();
            }
        }
        else
        {
            anim.Play("Move");

            transform.position = Vector2.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);

            if (spriteRenderer != null)
                spriteRenderer.flipX = patrolTarget.x < transform.position.x;

            if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
            {
                isWaiting = true;
                patrolWaitTimer = patrolWaitTime;
            }
        }
    }

    private void SetNewPatrolTarget()
    {
        patrolTarget = startPosition + Random.insideUnitCircle * patrolRadius;
    }

    private void ChaseBehavior(float distToPlayer)
    {
        anim.Play("Move");

        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (spriteRenderer != null)
            spriteRenderer.flipX = player.position.x < transform.position.x;

        if (distToPlayer <= attackRadius && currentState == EnemyState.Chase)
        {
            StartAttack();
        }
        else if (distToPlayer > aggroRadius && !isSonarAggroed)
        {
            StartPatrol();
        }
    }

    private void StartChase()
    {
        currentState = EnemyState.Chase;
        if (sfxSource && aggroClip) sfxSource.PlayOneShot(aggroClip);
    }

    private void StartPatrol()
    {
        currentState = EnemyState.Patrol;
        startPosition = transform.position;

        isWaiting = true;
        patrolWaitTimer = patrolWaitTime;
        SetNewPatrolTarget();
    }

    private void StartAttack()
    {
        if (currentState == EnemyState.Attack || currentState == EnemyState.Cooldown) return;

        currentState = EnemyState.Attack;
        anim.Play("Attack");

        if (sfxSource && attackClip) sfxSource.PlayOneShot(attackClip);

        if (attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(attackDamageDelay);

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= attackRadius + 0.5f)
        {
            GameManager.Instance.TakeDamage(1);
        }

        currentState = EnemyState.Cooldown;
        anim.Play("Idle");
        yield return new WaitForSeconds(postAttackCooldown);

        currentState = EnemyState.Chase;
    }

    public void TriggerSonarAggro(float duration)
    {
        isSonarAggroed = true;
        if (currentState == EnemyState.Patrol) StartChase();
        StartCoroutine(SonarTimer(duration));
    }

    private IEnumerator SonarTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSonarAggroed = false;
    }
}