using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [Header("Jump")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField, Range(0, 1)] private float jumpCutPercentage;
    private bool isJumping;
    private Timer jumpCoyoteTimer;

    [Header("Gravity")]
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fallingGravityStrength;
    private float originalGravityScale;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
    }

    private void Start()
    {
        originalGravityScale = rb.gravityScale;
        jumpCoyoteTimer = new Timer(jumpCoyoteTime);
    }

    void Update()
    {
        // Update timer
        jumpCoyoteTimer.Tick();
        if (pc.isGrounded)
        {
            jumpCoyoteTimer.Reset();
            rb.gravityScale = originalGravityScale;
        }

        FallGravity();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
            TryJump();
        else if (context.canceled)
            JumpCut();
    }

    private void TryJump()
    {
        bool canJump = pc.isGrounded || (!jumpCoyoteTimer.isOver && !isJumping);

        if (canJump)
        {
            isJumping = true;
            float force = jumpStrength - rb.velocity.y;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }

    private void JumpCut()
    {
        if (rb.velocity.y > 0 && isJumping)
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutPercentage), ForceMode2D.Impulse);

        isJumping = false;
    }

    private void FallGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = originalGravityScale * fallingGravityStrength;

            // If falling faster than max fall speed, fix the fall speed
            if (Mathf.Abs(rb.velocity.y) > maxFallSpeed)
            {
                Vector2 vel = rb.velocity;
                vel.y = -maxFallSpeed;
                rb.velocity = vel;
            }
        }
    }
}
