using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform _startPoint;

    [SerializeField] private float _rotationSpeed = 180f;

    private float _currentSpeed;

    private Rigidbody2D _rb;

    private float _moveInput;     // 🔥 тільки вперед/назад
    private float _rotationInput; // 🔥 тільки поворот

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

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        _moveInput = input.y;        // 🔥 тільки вперед/назад
        _rotationInput = input.x;   // 🔥 A/D → поворот
    }

    public void OnMoveCanceled(InputValue value)
    {
        _moveInput = 0f;
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

    private void RotateByInput()
    {
        float rotation = -_rotationInput * _rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotation);
    }

    private void Move()
    {
        Vector2 forward = transform.right;

        Vector2 move = forward * _moveInput * _currentSpeed;

        _rb.MovePosition(_rb.position + move * Time.fixedDeltaTime);
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