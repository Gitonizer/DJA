using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Modes")]
    public MovementType MovementType;

    [Header("Movement Values")]
    public float Speed;
    public float RigidBodySpeed;
    public float CameraLookSensitivity;
    public float CameraZoomSensitivity;
    public float JumpValue;

    [Header("GameObject Instances")]
    public GameObject PlayerModel;
    public GameObject CameraHolder;
    public GameObject Camera;

    private GameManager _gameManager;
    private Animator _animator;
    private float _axisForward;
    private float _axisStrafe;
    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private Vector3 _turn;
    private Vector3 _cameraZoom;
    private Vector3 _gravityVector;

    private const float GRAVITY = 6f;

    private const float MAX_ANIMATION_SPEED = 6f;

    private const float CAMERA_ZOOM_X_MIN = -1f;
    private const float CAMERA_ZOOM_X_MAX = -0f;
    private const float CAMERA_ZOOM_Z_MIN = -3.0f;
    private const float CAMERA_ZOOM_Z_MAX = 0.36f;

    private const float CAMERA_TURN_MINY = -70f;
    private const float CAMERA_TURN_MAXY = 50f;

    private bool _jumpPressed;
    private bool _pauseToggle;

    private float _sprintFactor;
    private bool _fire;
    private bool _onCooldown;

    private ShootClass _shootClass;

    private InputManager _inputManager;

    private void Awake()
    {
        _gameManager = GetComponentInParent<GameManager>();
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _shootClass = GetComponent<ShootClass>();
        _inputManager = _gameManager.InputManager;
        _turn = new Vector3();
        _cameraZoom = new Vector3();
        _gravityVector = Vector3.zero;
        _jumpPressed = false;
        _sprintFactor = 1f;
        _fire = false;
        _onCooldown = false;
        _pauseToggle = false;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (transform.position.y < -30f)
            _gameManager.RestartLevel();

        Inputs();

        ManagePause();
        Animate();
        Look();

        if (MovementType == MovementType.CharacterController) Move();
    }

    private void FixedUpdate()
    {
        if (MovementType == MovementType.Rigidbody) Move();
        Fire();
    }

    private void Inputs()
    {
        // look
        if (!_gameManager.IsPaused)
        {
            _turn.x += _inputManager.OnLookX() * (!_inputManager.IsUsingNewInputSystem ? CameraLookSensitivity : 1f);
            _turn.y += _inputManager.OnLookY() * (!_inputManager.IsUsingNewInputSystem ? CameraLookSensitivity : 1f);

            _turn.y = Mathf.Clamp(_turn.y, CAMERA_TURN_MINY, CAMERA_TURN_MAXY);
        }

        // move
        _axisForward = _inputManager.OnMoveVertical();
        _axisStrafe = _inputManager.OnMoveHorizontal();
        _jumpPressed = _inputManager.OnJump();
        _fire = _inputManager.OnFire();
        _sprintFactor = _inputManager.OnSprint() ? 1.3f : 1f;

        // zoom camera
        _cameraZoom = new Vector3(_inputManager.OnMouseScrolling() * 0.5f, 0f, _inputManager.OnMouseScrolling());

        // pause game
        _pauseToggle = _inputManager.OnPause();
    }

    private void Animate()
    {
        float currentSpeed = Mathf.Abs(Mathf.Abs(_axisForward) >= Mathf.Abs(_axisStrafe) ? _axisForward : _axisStrafe);

        _animator.SetFloat("Speed", SpeedToAnimationSpeed(currentSpeed));
        _animator.SetFloat("MotionSpeed", currentSpeed * _sprintFactor);
        _animator.SetBool("Jump", _jumpPressed);
        _animator.SetBool("Grounded", IsGrounded());
        _animator.SetBool("FreeFall", !IsGrounded());
    }

    private void Look()
    {
        transform.localRotation = Quaternion.Euler(0, _turn.x, 0);
        CameraHolder.transform.localRotation = Quaternion.Euler(-_turn.y, 0, 0);
        Camera.transform.localPosition += _cameraZoom * CameraZoomSensitivity;
        Camera.transform.localPosition = new Vector3(Mathf.Clamp(Camera.transform.localPosition.x, CAMERA_ZOOM_X_MIN, CAMERA_ZOOM_X_MAX), 0f, Mathf.Clamp(Camera.transform.localPosition.z, CAMERA_ZOOM_Z_MIN, CAMERA_ZOOM_Z_MAX));
    }

    private void Move()
    {
        _characterController.enabled = MovementType == MovementType.CharacterController;

        //jumping
        if (IsGrounded())
        {
            _gravityVector = -GRAVITY * Vector3.up;

            if (_jumpPressed)
            {
                if (MovementType == MovementType.CharacterController)
                    _gravityVector = Vector3.up * JumpValue * 0.7f;
                else
                {
                    _rigidbody.AddForce(JumpValue * 1.1f * Vector3.up, ForceMode.Impulse);
                }
            }
        }
        else
            _gravityVector += MovementType == MovementType.CharacterController  ?
                -GRAVITY * 2f * Time.deltaTime * Vector3.up :
                Vector3.zero;

        //rotating
        if (_axisForward != 0 || _axisStrafe != 0) // only update player model rotation with button press
            PlayerModel.transform.localRotation = Quaternion.LookRotation(new Vector3(_axisStrafe, 0, _axisForward), Vector3.up);

        //moving
        Vector3 playerDirection = transform.forward * _axisForward + transform.right * _axisStrafe;
        Vector3 movement = _sprintFactor * (_inputManager.IsUsingNewInputSystem ? playerDirection : playerDirection.normalized);

        MoveCharacter(movement, _gravityVector);
    }

    private void Fire()
    {
        if(_fire && !_onCooldown)
        {
            _onCooldown = true;

            StartCoroutine(_shootClass.ShootBullet(() =>
            {
                _onCooldown = false;
            }));
        }
    }

    private float SpeedToAnimationSpeed(float currentSpeed)
    {
        return currentSpeed * MAX_ANIMATION_SPEED;
    }

    private bool IsGrounded()
    {
        return MovementType == MovementType.CharacterController ?
            _characterController.isGrounded :
            Physics.Raycast(PlayerModel.transform.position, -Vector3.up, 0.1f);
    }

    private void MoveCharacter(Vector3 horizontalMovement, Vector3 verticalMovement)
    {
        if (MovementType == MovementType.CharacterController)
        {
            _characterController.Move(horizontalMovement * Speed * Time.deltaTime);
            _characterController.Move(verticalMovement * Speed * Time.deltaTime);
        }
        else
        {
            Vector3 movement = new Vector3(horizontalMovement.x, 0f, horizontalMovement.z) * RigidBodySpeed;
            _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * movement);
        }
    }

    private void ManagePause()
    {
        if (_pauseToggle)
            _gameManager.TogglePause();
    }

    // UI INPUT
}
