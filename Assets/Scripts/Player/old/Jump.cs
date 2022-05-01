using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    private Movement movement;

    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpMultiplier = 1.5f;
    [SerializeField] private float maxSpeedMultiplier = 1.5f;

    private float originalGravScale;
    [SerializeField] private float slidingGravScale;

    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        originalGravScale = rb.gravityScale;
    }

    public void OnJump(float direction)
    {
        if (isJumping) return;

        ResetGravity();
        isJumping = true;

        float maxJumpForce = jumpForce * maxJumpMultiplier;
        float maxJumpSpeed = movement.maxMoveSpeed * maxSpeedMultiplier;

        float slope = (jumpForce - maxJumpForce) / (movement.maxMoveSpeed - maxJumpSpeed);
        float jumpScale = slope * (rb.velocity.x - movement.maxMoveSpeed) + jumpForce;

        float scaledJumpForce = Mathf.Clamp(jumpScale, jumpForce, maxJumpForce);
        rb.AddForce(Vector2.up * scaledJumpForce, ForceMode2D.Impulse);
    }

    public void OnFall(float direction)
    {
        ResetGravity();
        isJumping = false;

        movement.OnAirMove(direction);
    }

    public void OnWallJump(float direction)
    {
        if (isJumping) return;

        ResetGravity();
        isJumping = true;

        Vector2 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;
        OnJump(direction);
    }

    public void OnWallSlide()
    {
        if (rb.velocity.y < 0)
            rb.gravityScale = slidingGravScale;
    }
    public void ResetGravity() => rb.gravityScale = originalGravScale;

    public bool IsJumping() => isJumping;
}
