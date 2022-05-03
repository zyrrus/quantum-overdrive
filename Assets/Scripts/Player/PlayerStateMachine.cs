using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    /* References */

    private PlayerInputAction playerInputAction;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform respawnPoint;
    private LevelManager lm;
    [SerializeField] private Transform flipRoot;
    [SerializeField] private Animator animator;


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

    private float originalGravityScale;


    /* Parameters */

    [Header("Movement")]
    [SerializeField] private float softMaxSpeed; // Player can naturally run up to this speed
    [SerializeField] private float bonusMaxSpeed; // Player will get bonuses when they reach this speed (higher than simple)
    [SerializeField] private float acceleration;
    [SerializeField] private float stoppingSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceBonus;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashRecoilForce;
    [SerializeField] private float dashEffectiveTime;
    [SerializeField] private float dashCooldownTime;
    [SerializeField] private float bounceForce;

    [Header("Wall Slide")]
    [SerializeField] private float wallSlideGravityScale;


    /* Flags */

    private bool isGrounded;
    private bool isFalling;
    private bool isJumping;
    private bool isDashing;
    private bool isLaunched;
    private bool requireNewJumpPress;
    private bool requireNewDashPress;
    private bool isTouchingWall;
    private float facingDirection = 1;


    /* Getters + Setters */

    // References
    public Rigidbody2D Rb { get => rb; }
    public Transform RespawnPoint { get => respawnPoint; }
    public Animator Animator { get => animator; }

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
    public float OriginalGravityScale { get => originalGravityScale; }

    // Parameters
    public float SoftMaxSpeed { get => softMaxSpeed; }
    public float BonusMaxSpeed { get => bonusMaxSpeed; }
    public float Acceleration { get => acceleration; }
    public float StoppingSpeed { get => stoppingSpeed; }
    public float JumpForce { get => jumpForce; }
    public float JumpForceBonus { get => jumpForceBonus; }
    public float DashForce { get => dashForce; }
    public float DashRecoilForce { get => dashRecoilForce; }
    public float BounceForce { get => bounceForce; }
    public float WallSlideGravityScale { get => wallSlideGravityScale; }

    // Flags
    public bool IsGrounded { get => isGrounded; }
    public bool IsFalling { get => isFalling; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool IsLaunched { get => isLaunched; set => isLaunched = value; }
    public bool RequireNewJumpPress { get => requireNewJumpPress; set => requireNewJumpPress = value; }
    public bool RequireNewDashPress { get => requireNewDashPress; set => requireNewDashPress = value; }
    public bool IsTouchingWall { get => isTouchingWall; }
    public float FacingDirection { get => facingDirection; }


    /* MonoBehaviour */

    private void Awake()
    {
        // Get components

        playerInputAction = new PlayerInputAction();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        lm = FindObjectOfType<LevelManager>();

        // Initialize status

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
        dashEffectiveTimer = new Timer(dashEffectiveTime);
        dashCooldownTimer = new Timer(dashCooldownTime);
        originalGravityScale = rb.gravityScale;

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
        // Update flags - Maybe want to call these in Get
        UpdateIsGrounded();
        UpdateIsFalling();
        UpdateIsDashing();
        UpdateIsTouchingWall();
        UpdateFacingDirection();
        isLaunched = false;


        // Update timers
        if (!dashEffectiveTimer.isOver) dashEffectiveTimer.Tick();
        if (!dashCooldownTimer.isOver) dashCooldownTimer.Tick();

        Flip();
        currentState.UpdateStates();
    }

    private void OnDrawGizmos()
    {
        Vector3 dashInput3 = new Vector3(currentDashInput.x, currentDashInput.y, 0);
        Gizmos.DrawLine(transform.position, transform.position + dashInput3);
    }

    public void Die()
    {
        // Reset everything
        isGrounded = false;
        isFalling = false;
        isJumping = false;
        isDashing = false;
        isLaunched = false;
        requireNewJumpPress = false;
        requireNewDashPress = false;
        isTouchingWall = false;

        // Move to respawn point
        transform.position = respawnPoint.position;

        // Reset level
        lm.ResetLevel();
    }

    private void Flip()
    {
        Vector3 scale = flipRoot.localScale;
        scale.x = facingDirection;
        flipRoot.localScale = scale;
    }

    /* Flag utilities */

    private void UpdateIsGrounded()
    {
        Vector3 offset = Vector3.down * 0.01f;
        Vector3 leftCorner = playerCollider.bounds.min + offset;
        Vector3 rightCorner = leftCorner + (Vector3.right * playerCollider.bounds.size.x);

        bool hitLeft = Physics2D.Raycast(leftCorner, Vector2.down, 0.01f, groundLayer.value).collider != null;
        bool hitRight = Physics2D.Raycast(rightCorner, Vector2.down, 0.01f, groundLayer.value).collider != null;

        Color left = (hitLeft) ? Color.green : Color.red;
        Color right = (hitRight) ? Color.green : Color.red;

        Debug.DrawRay(leftCorner, Vector2.down * 0.01f, left, 0.001f);
        Debug.DrawRay(rightCorner, Vector2.down * 0.01f, right, 0.001f);

        isGrounded = hitLeft || hitRight;
    }

    private void UpdateIsFalling() => isFalling = !isGrounded && rb.velocity.y < 0;

    private void UpdateIsTouchingWall()
    {
        int numRays = 4;

        float spaceBetweenRays = (playerCollider.bounds.size.y * 0.9f) / (numRays - 1);
        Vector3 offset = (Vector3.right * Mathf.Sign(facingDirection) * 0.01f) + (0.05f * playerCollider.bounds.size.y * Vector3.down);
        Vector3 topCorner = playerCollider.bounds.center + offset;
        topCorner.x += Mathf.Sign(facingDirection) * playerCollider.bounds.extents.x;
        topCorner.y += playerCollider.bounds.extents.y;

        // Check the rays
        for (int i = 0; i < numRays; i++)
        {
            Vector3 rayStart = topCorner + (i * spaceBetweenRays * Vector3.down);

            Debug.DrawRay(rayStart, Vector2.right * Mathf.Sign(facingDirection), Color.blue, 0.001f);

            if (Physics2D.Raycast(rayStart, Vector2.right * Mathf.Sign(facingDirection), 0.1f, groundLayer.value).collider != null)
            {
                isTouchingWall = true;
                return;
            }
        }

        isTouchingWall = false;
    }

    private void UpdateFacingDirection()
    {
        // If x vel == 0, let move input take care of it
        if (rb.velocity.x < -0.1f)
            facingDirection = -1;
        else if (rb.velocity.x > 0.1f)
            facingDirection = 1;
    }

    private void UpdateIsDashing()
    {
        if (dashEffectiveTimer.isOver) isDashing = false;
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


    /* Utilites */

    public void KillXVelocity()
    {
        Vector2 vel = rb.velocity;
        vel.x = 0;
        rb.velocity = vel;
    }

    public void KillYVelocity()
    {
        Vector2 vel = rb.velocity;
        vel.y = 0f;
        rb.velocity = vel;
    }

    public void ReduceXVelocity(float factor)
    {
        Vector2 vel = rb.velocity;
        vel.x = Mathf.MoveTowards(vel.x, 0, factor);
        rb.velocity = vel;
    }

    public void ReduceYVelocity(float factor)
    {
        Vector2 vel = rb.velocity;
        vel.y = Mathf.MoveTowards(vel.y, 0, factor);
        rb.velocity = vel;
    }
}
