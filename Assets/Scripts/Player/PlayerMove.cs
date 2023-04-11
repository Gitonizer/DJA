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

    private const float CAMERA_ZOOM_MIN = -3.0f;
    private const float CAMERA_ZOOM_MAX = 0.36f;

    private bool _jumpPressed;

    private float _sprintFactor;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _turn = new Vector3();
        _cameraZoom = new Vector3();
        _gravityVector = Vector3.zero;
        _jumpPressed = false;
        _sprintFactor = 1f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        OldSystemInputs();

        Animate();
        Look();

        if (MovementType == MovementType.CharacterController) Move();
    }

    private void FixedUpdate()
    {
        if (MovementType == MovementType.Rigidbody) Move();
    }

    private void OldSystemInputs()
    {
        // look
        _turn.x += Input.GetAxis("Mouse X") * CameraLookSensitivity;
        _turn.y += Input.GetAxis("Mouse Y") * CameraLookSensitivity;
        
        // move
        _axisForward = Input.GetAxis("Vertical");
        _axisStrafe = Input.GetAxis("Horizontal");
        _jumpPressed = Input.GetButton("Jump");
        _sprintFactor = Input.GetKey(KeyCode.LeftShift) ? 1.3f : 1f;

        // zoom camera
        _cameraZoom = new Vector3(0f, 0f, Input.GetAxis("Mouse ScrollWheel"));

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
        Camera.transform.localPosition = new Vector3(0f, 0f, Mathf.Clamp(Camera.transform.localPosition.z, CAMERA_ZOOM_MIN, CAMERA_ZOOM_MAX));
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
                    _gravityVector = Vector3.up * JumpValue;
                else
                {
                    _rigidbody.AddForce(JumpValue * Vector3.up, ForceMode.Impulse);
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
        Vector3 movement = _sprintFactor * ((transform.forward * _axisForward + transform.right * _axisStrafe).normalized);

        MoveCharacter(movement, _gravityVector);
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

    // make code more clear
    // rigidbody mode
    // create player controller movement base to inherit? maybe
}
