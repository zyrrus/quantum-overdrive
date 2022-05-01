using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    /* References */

    private PlayerInputAction playerInputAction;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    [SerializeField] private Camera mainCamera;
    // rotation parent
    // animator


    /* Inputs */

    private float currentMovementInput;
    private Vector2 currentDashInput;
    private bool isMovementPressed;
    private bool isDashPressed;
    private bool isJumpPressed;


    /* Status */

    private PlayerBaseState currentState;
    private PlayerStateFactory states;

    private float currentMaxSpeed = 0;
    private Timer dashEffectiveTimer;
    private Timer dashCooldownTimer;


    /* Parameters */

    [Header("Movement")]
    [SerializeField] private float softMaxSpeed; // Player can naturally run up to this speed
    [SerializeField] private float bonusMaxSpeed; // Player will get bonuses when they reach this speed (higher than simple)
    [SerializeField] private float acceleration; // NOTE: not implemented
    [SerializeField] private float stoppingSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceBonus;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashRecoilForce;
    [SerializeField] private float dashEffectiveTime;
    [SerializeField] private float dashCooldownTime;


    /* Flags */

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool requireNewJumpPress;
    [SerializeField] private bool requireNewDashPress;
    [SerializeField] private float facingDirection = 1;


    /* Getters + Setters */

    // References
    public Rigidbody2D Rb { get => rb; }

    // Inputs
    public float MovementInput { get => currentMovementInput; }
    public Vector2 DashInput { get => currentDashInput; }
    public bool IsMovementPressed { get => isMovementPressed; }
    public bool IsJumpPressed { get => isJumpPressed; }
    public bool IsDashPressed { get => isDashPressed; }

    // Status
    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public float CurrentMaxSpeed { get => currentMaxSpeed; set => currentMaxSpeed = value; }
    public Timer DashEffectiveTimer { get => dashEffectiveTimer; }
    public Timer DashCooldownTimer { get => dashCooldownTimer; }

    // Parameters
    public float SoftMaxSpeed { get => softMaxSpeed; }
    public float BonusMaxSpeed { get => bonusMaxSpeed; }
    public float Acceleration { get => acceleration; }
    public float StoppingSpeed { get => stoppingSpeed; }
    public float JumpForce { get => jumpForce; }
    public float JumpForceBonus { get => jumpForceBonus; }
    public float DashForce { get => dashForce; }
    public float DashRecoilForce { get => dashRecoilForce; }

    // Flags
    public bool IsGrounded { get => isGrounded; }
    public bool IsFalling { get => isFalling; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool RequireNewJumpPress { get => requireNewJumpPress; set => requireNewJumpPress = value; }
    public bool RequireNewDashPress { get => requireNewDashPress; set => requireNewDashPress = value; }
    public float FacingDirection { get => facingDirection; }


    /* MonoBehaviour */

    private void Awake()
    {
        // Get components

        playerInputAction = new PlayerInputAction();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        // Initialize status

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
        dashEffectiveTimer = new Timer(dashEffectiveTime);
        dashCooldownTimer = new Timer(dashCooldownTime);

        // Tie input handlers to PlayerInputAction

        playerInputAction.Player.Move.started += OnMoveInput;
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;

        playerInputAction.Player.DashAbsDirection.started += OnDashAbsInput;
        playerInputAction.Player.DashAbsDirection.performed += OnDashAbsInput;
        playerInputAction.Player.DashAbsDirection.canceled += OnDashAbsInput;
        playerInputAction.Player.DashRelDirection.started += OnDashRelInput;
        playerInputAction.Player.DashRelDirection.performed += OnDashRelInput;
        playerInputAction.Player.DashRelDirection.canceled += OnDashRelInput;

        playerInputAction.Player.Dash.started += OnDash;
        playerInputAction.Player.Dash.canceled += OnDash;

        playerInputAction.Player.Jump.started += OnJump;
        playerInputAction.Player.Jump.canceled += OnJump;
    }

    private void OnEnable() => playerInputAction.Enable();

    private void OnDisable() => playerInputAction.Disable();

    private void Update()
    {
        // Update flags
        UpdateIsGrounded();
        UpdateIsFalling();
        UpdateFacingDirection();

        // Update timers
        if (!dashCooldownTimer.isOver) dashCooldownTimer.Tick();

        currentState.UpdateStates();
    }

    private void OnDrawGizmos()
    {
        Vector3 dashInput3 = new Vector3(currentDashInput.x, currentDashInput.y, 0);
        Gizmos.DrawLine(transform.position, transform.position + dashInput3);
    }


    /* Flag utilities */

    private void UpdateIsGrounded()
    {
        Vector3 offset = Vector3.down * 0.01f;
        Vector3 leftCorner = playerCollider.bounds.min + offset;
        Vector3 rightCorner = leftCorner + (Vector3.right * playerCollider.bounds.size.x) + offset;

        bool hitLeft = Physics2D.Raycast(leftCorner, Vector2.down, 0.01f).collider != null;
        bool hitRight = Physics2D.Raycast(rightCorner, Vector2.down, 0.01f).collider != null;

        // Debug.DrawRay(leftCorner, Vector2.down * 0.1f, Color.red, 2);
        // Debug.DrawRay(rightCorner, Vector2.down * 0.1f, Color.blue, 2);

        isGrounded = hitLeft || hitRight;
    }

    private void UpdateIsFalling() => isFalling = !isGrounded && rb.velocity.y < 0;

    private void UpdateFacingDirection()
    {
        // If x vel == 0, let move input take care of it
        if (rb.velocity.x < -0.1f)
            facingDirection = -1;
        else if (rb.velocity.x > 0.1f)
            facingDirection = 1;
    }


    /* Input handlers */

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<float>();
        isMovementPressed = (currentMovementInput != 0);
        if (isMovementPressed) facingDirection = currentMovementInput;
    }

    public void OnDashAbsInput(InputAction.CallbackContext context)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 playerPos = new Vector2(screenPos.x, screenPos.y);
        Vector2 deltaPos = context.ReadValue<Vector2>() - playerPos;

        currentDashInput = deltaPos.normalized;
    }

    public void OnDashRelInput(InputAction.CallbackContext context)
    {
        currentDashInput = context.ReadValue<Vector2>().normalized;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        isDashPressed = context.ReadValueAsButton();
        requireNewDashPress = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }
}
