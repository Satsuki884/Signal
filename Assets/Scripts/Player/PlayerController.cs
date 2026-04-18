using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform _startPoint;

    // [SerializeField] private Animator _animator;
    // [SerializeField] private AudioManager audioManager;

    private float _currentSpeed;

    private Rigidbody2D _rb;

    private Vector2 _moveInput;

    private Vector2 prevPosition;
    private Vector2 lastFrameVelocity;

    [SerializeField] private float movementThreshold = 0.01f; // п≥длаштуй: 0.01f або 0.05f
    [SerializeField] private float stopDelay = 0.08f; // ск≥льки секунд без руху, щоб вважати зупинку
    private float stationaryTimer = 0f;
    private bool lastEngineState = false;

    private void Awake()
    {
        if (_rb == null)
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
        _moveInput = value.Get<Vector2>();
    }

    public void OnMoveCanceled(InputValue value)
    {
        _moveInput = Vector2.zero;
    }


    private void FixedUpdate()
    {
        Move();

        // ќбчислюЇмо "реальну" швидк≥сть по зм≥н≥ позиц≥њ м≥ж FixedUpdate викликами
        Vector2 newPos = _rb.position;
        lastFrameVelocity = (newPos - prevPosition) / Time.fixedDeltaTime;
        prevPosition = newPos;

        if (!Mouse.current.leftButton.isPressed)
        {
            RotateToMovement();
        }
    }



    private void Update()
    {
        FlipToMouse();
        UpdateAnimations();

        // використовуЇмо magnitude (не sqrMagnitude) дл€ порогу Ч зручн≥ше читати
        float speed = lastFrameVelocity.magnitude;

        // Debug дл€ налагодженн€ Ч прибери коли все ок
        Debug.Log($"vel: {speed:F4}, stationaryTimer: {stationaryTimer:F3}");

        if (speed > movementThreshold)
        {
            // Ї рух Ч скидаЇмо таймер ≥ вмикаЇмо двигун
            stationaryTimer = 0f;
            SetEngineIfNeeded(true);
        }
        else
        {
            // немаЇ руху Ч накопичуЇмо час без руху
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer >= stopDelay)
                SetEngineIfNeeded(false);
            else
                SetEngineIfNeeded(true); // поки не пройшов stopDelay Ч вважай що ще рух
        }

    }
    private void SetEngineIfNeeded(bool state)
    {
        if (AudioManager.Instanse == null) return;
        if (lastEngineState == state) return;

        lastEngineState = state;
        AudioManager.Instanse.SetEngineState(state);
    }

    private void FlipToMouse()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 direction = mouseWorld - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void UpdateAnimations()
    {
        float speed = _moveInput.magnitude;
        // _animator.SetFloat("Speed", speed);
    }

    private void Move()
    {
        Vector2 forward = transform.right;

        Vector2 right = new Vector2(forward.y, -forward.x);

        Vector2 move = (forward * _moveInput.y + right * _moveInput.x) * _currentSpeed;

        _rb.MovePosition(_rb.position + move * Time.fixedDeltaTime);
    }

    private void RotateToMovement()
    {
        if (_moveInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }

}