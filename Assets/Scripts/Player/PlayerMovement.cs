using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Flag groundFlag;
    [SerializeField] private float rotationDuration = 0.1f;

    private Vector2 inputTarget;
    private Vector2 lastTarget = new Vector2(1, 0);
    private bool isGrounded;
    private bool isFacingRight = true;

    [Header("Move")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

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


    [Header("Stopping Friction")]
    [SerializeField, Range(0, 1)] private float frictionStrength;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpCoyoteTimer = new Timer(jumpCoyoteTime);
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        // Update flags and timers
        isGrounded = groundFlag.IsTriggered();

        jumpCoyoteTimer.Tick();

        if (isGrounded)
        {
            jumpCoyoteTimer.Reset();
        }

        // Perform actions
        Run();
        StoppingFriction();
        FallGravity();
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
        bool canJump = isGrounded || (!jumpCoyoteTimer.isOver && !isJumping);

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
        if (rb.velocity.y > 0) rb.gravityScale = originalGravityScale;
        else
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

    private void StoppingFriction()
    {
        if (isGrounded && Mathf.Abs(inputTarget.x) < 0.01f)
        {
            float friction = Mathf.Min(Mathf.Abs(rb.velocity.x), frictionStrength);
            friction *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.left * friction, ForceMode2D.Impulse);
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (inputTarget.x != 0) lastTarget.x = inputTarget.x;
        if (inputTarget.y != 0) lastTarget.y = inputTarget.y;

        inputTarget = context.ReadValue<Vector2>();

        if (IsFacingWrongWay()) FlipCharacter();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
            Jump();
        else if (context.canceled)
            JumpCut();
    }

    private bool IsFacingWrongWay() => lastTarget.x * inputTarget.x < 0;
    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        StartCoroutine(FlipCharLerp());
    }

    private IEnumerator FlipCharLerp()
    {
        int facingSign = (isFacingRight) ? 1 : -1;

        Quaternion endGoal = Quaternion.LookRotation(Vector3.forward * facingSign, Vector3.up);

        float timeElapsed = 0;
        while (timeElapsed < rotationDuration)
        {
            Vector3 direction = Vector3.Slerp(Vector3.forward, -Vector3.forward, timeElapsed / rotationDuration);
            Quaternion rotation = Quaternion.LookRotation(direction * -facingSign, Vector3.up);

            playerModel.transform.rotation = rotation;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        playerModel.transform.rotation = endGoal;
    }

    private void OnDrawGizmos()
    {
        float rad = 2;
        Vector2 root = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(root + (inputTarget * rad), 0.1f);
    }
}
