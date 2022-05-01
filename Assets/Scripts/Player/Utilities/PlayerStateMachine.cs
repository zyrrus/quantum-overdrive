using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    /* References */

    private PlayerInputAction playerInputAction;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
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


    /* Parameters */

    [Header("Movement")]
    [SerializeField] private float softMaxSpeed; // Player can naturally run up to this speed
    [SerializeField] private float bonusMaxSpeed; // Player will get bonuses when they reach this speed (higher than simple)
    [SerializeField] private float acceleration; // NOTE: not implemented
    [SerializeField] private float stoppingSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceBonus;

    // [Header("Dash")]


    /* Flags */

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool requireNewJumpPress;
    // facingDirection



    /* Getters + Setters */
    /*#region*/

    // References
    public Rigidbody2D Rb { get => rb; }

    // Inputs
    public float MovementInput { get => currentMovementInput; }
    public Vector2 DashInput { get => currentDashInput; }
    public bool IsMovementPressed { get => isMovementPressed; }
    public bool IsJumpPressed { get => isJumpPressed; }

    // Status
    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public float CurrentMaxSpeed { get => currentMaxSpeed; set => currentMaxSpeed = value; }

    // Parameters
    public float SoftMaxSpeed { get => softMaxSpeed; }
    public float BonusMaxSpeed { get => bonusMaxSpeed; }
    public float Acceleration { get => acceleration; }
    public float StoppingSpeed { get => stoppingSpeed; }
    public float JumpForce { get => jumpForce; }
    public float JumpForceBonus { get => jumpForceBonus; }

    // Flags
    public bool IsGrounded { get => isGrounded; }
    public bool IsFalling { get => isFalling; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool RequireNewJumpPress { get => requireNewJumpPress; set => requireNewJumpPress = value; }


    /* #endregion */


    /* MonoBehaviour methods */
    /*#region*/

    private void Awake()
    {
        // Get components

        playerInputAction = new PlayerInputAction();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();


        // Initialize states

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();


        // Tie input handlers to PlayerInputAction

        playerInputAction.Player.Move.started += OnMoveInput;
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;

        playerInputAction.Player.DashDirection.started += OnDashInput;
        playerInputAction.Player.DashDirection.performed += OnDashInput;
        playerInputAction.Player.DashDirection.canceled += OnDashInput;

        playerInputAction.Player.Dash.started += OnDash;
        playerInputAction.Player.Dash.canceled += OnDash;

        playerInputAction.Player.Jump.started += OnJump;
        playerInputAction.Player.Jump.canceled += OnJump;
    }

    private void OnEnable() => playerInputAction.Enable();
    private void OnDisable() => playerInputAction.Disable();

    private void Update()
    {
        UpdateIsGrounded();
        isFalling = !isGrounded && rb.velocity.y < 0;

        currentState.UpdateStates();
    }
    /*#endregion*/

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


    /* Input handlers */
    /*#region*/

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<float>();
        isMovementPressed = (currentMovementInput != 0);
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        currentDashInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        isDashPressed = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }
    /*#endregion*/
}
