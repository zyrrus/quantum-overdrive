using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Constants
    private readonly float diagonalInput = Mathf.Sqrt(2) / 2;

    [Header("References")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Flag groundFlag;

    private Vector2 inputTarget;
    private Vector2 lastTarget = new Vector2(1, 0);
    private bool isGrounded;

    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float queuedJumpTime;
    private bool pressedJump;
    private bool jumpQueued;
    private bool justJumped;
    private Timer jumpCoyoteTimer;
    private Timer queuedJumpTimer;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpCoyoteTimer = new Timer(jumpCoyoteTime);
        queuedJumpTimer = new Timer(queuedJumpTime);
    }

    private void Update()
    {
        // Update flags and timers
        isGrounded = groundFlag.IsTriggered();

        jumpCoyoteTimer.Tick();
        queuedJumpTimer.Tick();

        if (isGrounded)
        {
            jumpCoyoteTimer.Reset();
            justJumped = false;
            jumpQueued = false;
        };

        // Apply forces
        Vector2 newVel = new Vector2();

        newVel += Run();
        newVel += Jump();

        rb.velocity = newVel;
    }

    private Vector2 Run()
    {
        Vector2 vel = inputTarget * moveSpeed;
        vel.y = 0;
        return vel;
    }

    private Vector2 Jump()
    {
        Vector2 vel = new Vector2();

        if (CanJump())
        {
            Debug.Log($"Jump! {transform.position.y} \nisGrounded: {isGrounded}; \npressedJump: {pressedJump}; \njumpQueued: {jumpQueued}; \njustJumped: {justJumped}; \njumpCoyoteTimer: {jumpCoyoteTimer.isOver}; \nqueuedJumpTimer: {queuedJumpTimer.isOver}");
            vel.y = jumpStrength;
            jumpQueued = false;
            justJumped = true;
        }

        return vel;
    }

    private bool CanJump()
    {
        if (justJumped) return false;

        bool wasGrounded = !jumpCoyoteTimer.isOver;

        pressedJump = inputTarget.y >= diagonalInput;

        if (isGrounded)
        {
            jumpCoyoteTimer.Reset();
            if (pressedJump || jumpQueued)
            {
                // Debug.Log($"Grounded. Pressed or Queued.");
                return true;
            }
        }
        else if (pressedJump)
        {
            if (wasGrounded)
            {
                // Debug.Log($"Not Grounded. Pressed, was grounded and didn't just jump");
                return true;
            }
            else if (!jumpQueued)
            {
                // Debug.Log($"Not Grounded. Pressed, Queued");
                queuedJumpTimer.Reset();
                jumpQueued = true;
                return false;
            }
        }

        return false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (inputTarget.x != 0) lastTarget.x = inputTarget.x;
        if (inputTarget.y != 0) lastTarget.y = inputTarget.y;

        inputTarget = context.ReadValue<Vector2>();

        if (IsFacingWrongWay()) FlipCharacter();
    }

    private bool IsFacingWrongWay()
    {
        Debug.Log(lastTarget + " " + inputTarget);
        return lastTarget.x * inputTarget.x < 0;
    }

    private void FlipCharacter()
    {
        playerModel.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnDrawGizmos()
    {
        float rad = 2;
        Vector2 root = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(root + (inputTarget * rad), 0.1f);
    }
}
