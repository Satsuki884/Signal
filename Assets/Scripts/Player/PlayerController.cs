using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform _startPoint;

    [Header("Movement (Heavy)")]
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _deceleration = 1.5f;

    [Header("Rotation (Light)")]
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _rotationAcceleration = 800f;
    [SerializeField] private Animator _animator;

    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> _engineVFX;

    private float _currentSpeed;
    private float _currentVelocity = 0f;
    private float _currentRotationSpeed = 0f;

    private Rigidbody2D _rb;

    private float _moveInput;
    private float _rotationInput;

    private Vector2 prevPosition;
    private Vector2 lastFrameVelocity;

    [SerializeField] private float movementThreshold = 0.01f;
    [SerializeField] private float stopDelay = 0.08f;

    private float stationaryTimer = 0f;
    private bool lastEngineState = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        transform.position = _startPoint.position;
        prevPosition = _rb.position;
        SetParticles(false);
    }

    private void Start()
    {
        _currentSpeed = playerSO.WalkSpeed;

    }

    // 🔥 Input
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        _moveInput = input.y;
        _rotationInput = input.x;
    }

    public void OnMoveCanceled(InputValue value)
    {
        _moveInput = 0f;
        _rotationInput = 0f;
    }

    private void FixedUpdate()
    {
        Move();
        RotateByInputFixed(); // Вращаем тут!

        Vector2 newPos = _rb.position;
        lastFrameVelocity = (newPos - prevPosition) / Time.fixedDeltaTime;
        prevPosition = newPos;
    }

    private void Update()
    {
        // Оставляем в Update только визуал и логику анимаций
        UpdateAnimations();

        float speed = lastFrameVelocity.magnitude;

        if (speed > movementThreshold)
        {
            stationaryTimer = 0f;
            SetEngineIfNeeded(true);
            SetParticles(true);
        }
        else
        {
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer >= stopDelay)
            {
                SetEngineIfNeeded(false);
                SetParticles(false);
            }
            else
            {
                SetEngineIfNeeded(true);
                SetParticles(true);
            }
        }
    }

    // 🔥 ВАЖКИЙ РУХ (сильна інерція)
    private void Move()
    {
        float targetVelocity = _moveInput * _currentSpeed;

        float accel = Mathf.Abs(targetVelocity) > Mathf.Abs(_currentVelocity)
            ? _acceleration
            : _deceleration;

        _currentVelocity = Mathf.MoveTowards(
            _currentVelocity,
            targetVelocity,
            accel * Time.fixedDeltaTime
        );

        // щоб не повзло вічно
        if (Mathf.Abs(_currentVelocity) < 0.01f)
            _currentVelocity = 0f;

        Vector2 forward = transform.right;
        Vector2 move = forward * _currentVelocity;

        _rb.MovePosition(_rb.position + move * Time.fixedDeltaTime);
    }

    // 🔥 ЛЕГКИЙ ПОВОРОТ (менше інерції)
    private void RotateByInputFixed()
    {
        float targetRotationSpeed = _rotationInput * _rotationSpeed;

        float accel = Mathf.Abs(_rotationInput) > 0.01f
            ? _rotationAcceleration
            : _rotationAcceleration * 2f;

        _currentRotationSpeed = Mathf.MoveTowards(
            _currentRotationSpeed,
            targetRotationSpeed,
            accel * Time.fixedDeltaTime // Заменили Time.deltaTime на Time.fixedDeltaTime
        );

        // Крутим физическое тело, а не трансформ!
        _rb.MoveRotation(_rb.rotation - _currentRotationSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimations()
    {
        float normalizedSpeed = Mathf.InverseLerp(0, _currentSpeed, Mathf.Abs(_currentVelocity));

        if (_animator != null)
            _animator.SetFloat("Speed", normalizedSpeed);
    }

    private void SetEngineIfNeeded(bool state)
    {
        if (AudioManager.Instanse == null) return;
        if (lastEngineState == state) return;

        lastEngineState = state;
        AudioManager.Instanse.SetEngineState(state);
    }

    private void SetParticles(bool state)
    {
        foreach (var ps in _engineVFX)
        {
            if (state)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
            else
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1) мінімальна відносна швидкість — щоб не грати звук при легких дотиках
        float relVel = collision.relativeVelocity.magnitude;
        if (relVel < AudioManager.Instanse?.collisionMinRelativeVelocity) return;

        // 2) якщо інший об'єкт є джерелом шкоди — не граємо collision sound
        var other = collision.collider;

        // перевірка за компонентом Enemy
        if (other.GetComponent<Enemy>() != null) return;

        // перевірка за інтерфейсом IDamageDealer (якщо додали)
        // замість GetComponent(typeof(IDamageDealer))
        if (other.GetComponent<Enemy>() != null) return;


        // перевірка за тегом (якщо у тебе є теги для небезпечних об'єктів)
        if (other.CompareTag("Damage") || other.CompareTag("Enemy")) return;

        // Якщо пройшли всі фільтри — граємо випадковий collision звук
        if (AudioManager.Instanse != null)
        {
            AudioManager.Instanse.PlayCollisionSoundRandom(transform.position, Mathf.Clamp01(relVel / 10f));
        }
    }

}