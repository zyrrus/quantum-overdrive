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
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

    [Header("Jump")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float queuedJumpTime;
    private bool pressedJump;
    private bool jumpQueued = false;
    private bool isJumping;
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
            isJumping = false;
            jumpQueued = false;
        };


        Run();
        Jump();
    }

    private void Run()
    {
        float targetSpeed = inputTarget.x * maxSpeed;
        float diffFromMax = targetSpeed - rb.velocity.x;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * accel, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

    private void Jump()
    {
        if (CanJump())
        {
            float force = jumpStrength - rb.velocity.y;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            jumpQueued = false;
            isJumping = true;
        }

    }

    private bool CanJump()
    {
        bool wasGrounded = !jumpCoyoteTimer.isOver;

        pressedJump = inputTarget.y >= diagonalInput;

        Debug.Log(isGrounded + " " + pressedJump);
        if (isGrounded)
        {
            if (pressedJump || jumpQueued)
            {
                return true;
            }
        }
        // else if (pressedJump)
        // {
        //     if (wasGrounded)
        //     {
        //         // Debug.Log($"Not Grounded. Pressed, was grounded and didn't just jump");
        //         return true;
        //     }
        //     else if (!jumpQueued)
        //     {
        //         Debug.Log($"Queued up a jump");
        //         queuedJumpTimer.Reset();
        //         jumpQueued = true;
        //         return false;
        //     }
        // }

        return false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // if (!context.performed) return;

        if (inputTarget.x != 0) lastTarget.x = inputTarget.x;
        if (inputTarget.y != 0) lastTarget.y = inputTarget.y;

        inputTarget = context.ReadValue<Vector2>();

        if (IsFacingWrongWay()) FlipCharacter();
    }

    private bool IsFacingWrongWay() => lastTarget.x * inputTarget.x < 0;
    private void FlipCharacter() => playerModel.transform.Rotate(new Vector3(0, 180, 0));

    private void OnDrawGizmos()
    {
        float rad = 2;
        Vector2 root = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(root + (inputTarget * rad), 0.1f);
    }
}
