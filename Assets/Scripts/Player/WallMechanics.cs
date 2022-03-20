using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallMechanics : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private Rigidbody2D rb;
    private PlayerCore pc;

    private float originalGravityScale;
    private bool isTouchingWall;

    [Header("Wall Slide")]
    [SerializeField] private float fallingSpeed = 0;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallGrabTime;
    private Timer wallGrabTimer;
    private bool needToRestartWallTimer = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
    }

    private void Start()
    {
        originalGravityScale = rb.gravityScale;
        wallGrabTimer = new Timer(wallGrabTime);
    }

    private void Update()
    {
        // Update timer
        wallGrabTimer.Tick();

        if (needToRestartWallTimer)
        {
            wallGrabTimer.Reset();
            needToRestartWallTimer = false;
        }

        // Update flags
        isTouchingWall = pc.isHittingUpperWall && pc.isHittingLowerWall;
        bool isRunningIntoWall = pc.inputTarget.x * (pc.isFacingRight ? 1 : -1) > 0 && isTouchingWall;

        if (!pc.isGrounded && (!wallGrabTimer.isOver || isRunningIntoWall))
        {
            if (isRunningIntoWall) needToRestartWallTimer = true;

            // Stop the player for a second, but keep any upwards velocity
            Vector2 wallGrabVel = rb.velocity;
            wallGrabVel.y = Mathf.Max(0, rb.velocity.y);
            rb.velocity = wallGrabVel;

            // Slow down falling speed
            if (rb.velocity.y < 0.1f) rb.gravityScale = fallingSpeed;
        }
        else rb.gravityScale = originalGravityScale;

    }

    public void OnWallJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        bool isWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, layerMask);
        Vector2 jumpDir = new Vector2(isWallRight ? -1 : 1, 1).normalized;

        if (!pc.isGrounded && (isTouchingWall || !wallGrabTimer.isOver))
            rb.AddForce(jumpDir * wallJumpForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0.5f, 0, 0));
    }
}
