using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform _startPoint;

    [Header("Movement")]
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 4f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _rotationAcceleration = 300f;

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
    }

    private void Start()
    {
        _currentSpeed = playerSO.WalkSpeed;
    }

    // 🔥 Input (Vector2 WASD)
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

        Vector2 newPos = _rb.position;
        lastFrameVelocity = (newPos - prevPosition) / Time.fixedDeltaTime;
        prevPosition = newPos;
    }

    private void Update()
    {
        RotateByInput();
        UpdateAnimations();

        float speed = lastFrameVelocity.magnitude;

        if (speed > movementThreshold)
        {
            stationaryTimer = 0f;
            SetEngineIfNeeded(true);
        }
        else
        {
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer >= stopDelay)
                SetEngineIfNeeded(false);
            else
                SetEngineIfNeeded(true);
        }
    }

    // 🔥 ІНЕРЦІЯ РУХУ
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

        // щоб не "повзло" вічно
        if (Mathf.Abs(_currentVelocity) < 0.01f)
            _currentVelocity = 0f;

        Vector2 forward = transform.right;
        Vector2 move = forward * _currentVelocity;

        _rb.MovePosition(_rb.position + move * Time.fixedDeltaTime);
    }

    // 🔥 ІНЕРЦІЯ ПОВОРОТУ
    private void RotateByInput()
    {
        float targetRotationSpeed = _rotationInput * _rotationSpeed;

        _currentRotationSpeed = Mathf.MoveTowards(
            _currentRotationSpeed,
            targetRotationSpeed,
            _rotationAcceleration * Time.deltaTime
        );

        transform.Rotate(0f, 0f, -_currentRotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        float speed = Mathf.Abs(_moveInput);
        // _animator.SetFloat("Speed", speed);
    }

    private void SetEngineIfNeeded(bool state)
    {
        if (AudioManager.Instanse == null) return;
        if (lastEngineState == state) return;

        lastEngineState = state;
        AudioManager.Instanse.SetEngineState(state);
    }
}