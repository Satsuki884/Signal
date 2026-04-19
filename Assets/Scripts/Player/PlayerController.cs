using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 360f; // ������� � �������

    [Header("Movement")]
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform _startPoint;

    [Header("Inertia")]
    [SerializeField] private float velocitySmoothTime = 0.18f; // ��� ������������ (���)
    [SerializeField] private float maxSpeed = 5f; // ������� ���� �������� (�������� �� playerSO.WalkSpeed)
    [SerializeField] private float acceleration = 20f; // ��������� ����������� (0 = ��� ���������)
    private Vector2 velocitySmooth; // ref ��� SmoothDamp

    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private Vector2 prevPosition;
    private Vector2 lastFrameVelocity;

    [SerializeField] private float movementThreshold = 0.01f; // ��������: 0.01f ��� 0.05f
    [SerializeField] private float stopDelay = 0.08f; // ��� (�) ��� ���� ��� �������� �������
    private float stationaryTimer = 0f;
    private bool lastEngineState = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_startPoint != null)
            transform.position = _startPoint.position;

        prevPosition = _rb.position;
    }

    private void Start()
    {
        if (playerSO != null)
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

        // ���������� "�������" �������� �� ���� ������� �� FixedUpdate ���������
        Vector2 newPos = _rb.position;
        lastFrameVelocity = (newPos - prevPosition) / Time.fixedDeltaTime;
        prevPosition = newPos;

        // ���� ����� ����������� �� �������� ���� ������ ����� � ����������� ��� ����
        // if (!Mouse.current.leftButton.isPressed)
        // {
        //     RotateToMovement();
        // }
    }

    private void Update()
    {
        // ������� ������� �� ����� (�� �������)
        FlipToMouse();

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
                SetEngineIfNeeded(true); // ���� �� ������� stopDelay � ������ �� �� ���
        }

        // ��� ����� ��������� AudioManager.UpdateEnginePitch(...) ���� ������ ����� �����
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

        if (direction.sqrMagnitude < 0.0001f) return; // ������ ���� ������� � ������ �� ������

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;

        // ����������� ���� ���� �� ���� (�������)
        float maxDelta = rotationSpeed * Time.deltaTime;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxDelta);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void UpdateAnimations()
    {
        float speed = _moveInput.magnitude;
        // _animator.SetFloat("Speed", speed);
    }

    private void Move()
    {
        // ��������� �������� ���� � ���
        Vector2 forward = transform.right;
        Vector2 right = new Vector2(forward.y, -forward.x);

        // �������� ����� (�������������, ��� �������� �������� �� �������)
        Vector2 inputDir = (forward * _moveInput.y + right * _moveInput.x);
        Vector2 targetDir = inputDir.sqrMagnitude > 0.0001f ? inputDir.normalized : Vector2.zero;

        // ֳ����� �������� (������������� _currentSpeed ���� �, ������ maxSpeed)
        float targetSpeed = (_currentSpeed > 0f) ? _currentSpeed : maxSpeed;
        Vector2 targetVelocity = targetDir * targetSpeed;

        // ������ ���������� ������� �������� �� �������
        Vector2 newVelocity = Vector2.SmoothDamp(_rb.linearVelocity, targetVelocity, ref velocitySmooth, velocitySmoothTime);

        // ��������� ��������� ����������� (�����������)
        if (acceleration > 0f)
        {
            Vector2 delta = newVelocity - _rb.linearVelocity;
            float maxStep = acceleration * Time.fixedDeltaTime;
            if (delta.magnitude > maxStep)
                newVelocity = _rb.linearVelocity + delta.normalized * maxStep;
        }

        _rb.linearVelocity = newVelocity;
    }


    private void RotateToMovement()
    {
        if (_moveInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
            float maxDelta = rotationSpeed * Time.fixedDeltaTime;
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, angle, maxDelta);
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }
    }
}
