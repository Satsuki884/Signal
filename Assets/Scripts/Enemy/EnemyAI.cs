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
    public float breathCooldown = 1f;

    [Header("Components")]
    public Transform player;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    [Header("Audio (local sources optional)")]
    public AudioSource breathSource; // optional fallback
    public AudioSource sfxSource;    // optional fallback

    [Header("Debug")]
    public bool debugDrawAlways = false;

    // internal
    private Coroutine attackRoutine;
    private bool isSonarAggroed;
    private Vector2 startPosition;
    private Vector2 patrolTarget;
    private float patrolWaitTimer;
    private bool isWaiting;

    // breathing control
    private float lastBreathTime = -Mathf.Infinity;

    // ensure aggro sound plays only once per chase entry
    private bool aggroPlayed = false;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (AudioManager.Instanse == null)
        {
            Debug.LogWarning($"AudioManager not found in scene. Enemy {name} will use local audio fallback.");
        }
        else
        {
           
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

            if (dist <= breathRadius && Time.time - lastBreathTime >= breathCooldown)
            {
                lastBreathTime = Time.time;

                // Базова гучність залежно від відстані
                float volume = 1f - Mathf.Clamp01(dist / breathRadius);

                // Отримуємо множник з AudioManager (fallback = 1)
                float multiplier = 1f;
                if (AudioManager.Instanse != null)
                    multiplier = AudioManager.Instanse.enemyBreathVolumeMultiplier;

                // Підсилюємо і обмежуємо в діапазоні 0..1
                float finalVolume = Mathf.Clamp01(volume * multiplier);

                if (AudioManager.Instanse != null && AudioManager.Instanse.enemyBreathClip != null)
                {
                    // positional breathing sound
                    AudioManager.Instanse.PlaySFXAtPosition(AudioManager.Instanse.enemyBreathClip, transform.position, finalVolume, 1f);
                }
                else if (breathSource != null && breathSource.clip != null)
                {
                    breathSource.PlayOneShot(breathSource.clip, finalVolume);
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
            if (anim != null) anim.Play("Idle");
            patrolWaitTimer -= Time.deltaTime;

            if (patrolWaitTimer <= 0)
            {
                isWaiting = false;
                SetNewPatrolTarget();
            }
        }
        else
        {
            if (anim != null) anim.Play("Move");

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

    private void ChaseBehavior(float distToPlayer)
    {
        if (anim != null) anim.Play("Move");

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

        if (!aggroPlayed)
        {
            aggroPlayed = true; // гарантуємо, що агро‑звук грає лише один раз при вході в Chase

            var am = AudioManager.Instanse;

            if (am != null && am.enemyAggroClip != null)
            {
                // spatialBlend = 0f → не позиційний (PlayOneShot через SFXSource)
                am.PlaySFXAtPosition(am.enemyAggroClip, transform.position, 1f, 0f);
            }
            else if (sfxSource != null && sfxSource.clip != null)
            {
                sfxSource.PlayOneShot(sfxSource.clip);
            }
        }
    }


    private void StartPatrol()
    {
        currentState = EnemyState.Patrol;
        startPosition = transform.position;

        isWaiting = true;
        patrolWaitTimer = patrolWaitTime;
        SetNewPatrolTarget();

        aggroPlayed = false;
    }

    private void StartAttack()
    {
        if (currentState == EnemyState.Attack || currentState == EnemyState.Cooldown) return;

        currentState = EnemyState.Attack;
        if (anim != null) anim.Play("Attack");

        if (AudioManager.Instanse != null && AudioManager.Instanse.enemyAttackClip != null)
        {
            AudioManager.Instanse.PlaySFXAtPosition(AudioManager.Instanse.enemyAttackClip, transform.position, 1f, 0f);
        }
        else if (sfxSource != null && sfxSource.clip != null)
        {
            sfxSource.PlayOneShot(sfxSource.clip);
        }

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
        if (anim != null) anim.Play("Idle");
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

    private void SetNewPatrolTarget()
    {
        patrolTarget = startPosition + Random.insideUnitCircle * patrolRadius;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        bool isSelected = UnityEditor.Selection.Contains(gameObject);
#else
        bool isSelected = false;
#endif

        if (!debugDrawAlways && !isSelected) return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = new Color(0f, 0.5f, 1f, 0.12f);
        Gizmos.DrawWireSphere(transform.position, breathRadius);

        Gizmos.color = new Color(1f, 0.9f, 0f, 0.12f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
