using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlags : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D collider2d;

    [SerializeField] private float dashCooldownTime = 1.5f;
    private Timer dashCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();

        dashCooldown = new Timer(dashCooldownTime);
    }

    private void Update()
    {
        if (!dashCooldown.isOver) dashCooldown.Tick();
    }

    public bool IsGrounded()
    {
        Vector3 offset = Vector3.down * 0.01f;
        Vector3 leftCorner = collider2d.bounds.min + offset;
        Vector3 rightCorner = leftCorner + (Vector3.right * collider2d.bounds.size.x) + offset;

        bool hitLeft = Physics2D.Raycast(leftCorner, Vector2.down, 0.1f).collider != null;
        bool hitRight = Physics2D.Raycast(rightCorner, Vector2.down, 0.1f).collider != null;

        // Debug.DrawRay(leftCorner, Vector2.down * 0.1f, Color.red, 2);
        // Debug.DrawRay(rightCorner, Vector2.down * 0.1f, Color.blue, 2);

        return hitLeft || hitRight;
    }

    public bool IsFalling() => rb.velocity.y < 0;

    public bool CanDash() => dashCooldown.isOver;
    public void ResetDashTimer() => dashCooldown.Reset();

    public bool CanJump(float direction)
    {
        return IsGrounded() || MovingIntoWall(direction);
    }

    public bool MovingIntoWall(float direction)
    {
        int numRays = 4;

        float spaceBetweenRays = (collider2d.bounds.size.y * 0.9f) / (numRays - 1);
        Vector3 offset = (Vector3.right * Mathf.Sign(direction) * 0.01f) + (0.05f * collider2d.bounds.size.y * Vector3.down);
        Vector3 topCorner = collider2d.bounds.center + offset;
        topCorner.x += Mathf.Sign(direction) * collider2d.bounds.extents.x;
        topCorner.y += collider2d.bounds.extents.y;

        // Check the rays
        for (int i = 0; i < numRays; i++)
        {
            Vector3 rayStart = topCorner + (i * spaceBetweenRays * Vector3.down);

            Debug.DrawRay(rayStart, Vector2.right * Mathf.Sign(direction), Color.blue, 0.1f);

            if (Physics2D.Raycast(rayStart, Vector2.right * Mathf.Sign(direction), 0.1f).collider != null)
                return true;
        }

        return false;
    }

    public bool CanWalk()
    {
        return true;
    }

}
