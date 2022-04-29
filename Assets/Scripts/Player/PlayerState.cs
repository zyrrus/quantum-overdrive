using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // State
    public enum PState { Idle, Walk, Jump, Fall, Wall, Dash, Dead }
    private PState currentState = PState.Idle;

    // Inputs
    private float moveInput = 0;
    private bool didJump = false;
    private bool didDash = false;

    // References
    private PlayerFlags pf;
    private Movement movement;
    private Dash dash;
    private Jump jump;

    private bool isFacingRight = true;
    [SerializeField] private float wallPityTime;
    private Timer wallPityTimer;



    private void Awake()
    {
        pf = GetComponent<PlayerFlags>();
        movement = GetComponent<Movement>();
        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();

        wallPityTimer = new Timer(wallPityTime);

    }

    private void Update()
    {
        if (!wallPityTimer.isOver) wallPityTimer.Tick();

        UpdateInputs();
        currentState = UpdateState();

        Debug.Log(currentState);

        switch (currentState)
        {
            case PState.Idle:
                IdleState();
                break;
            case PState.Walk:
                WalkState();
                break;
            case PState.Jump:
                JumpState();
                break;
            case PState.Fall:
                FallState();
                break;
            case PState.Wall:
                WallState();
                break;
            case PState.Dash:
                DashState();
                break;
            case PState.Dead:
                DeadState();
                break;
        }
    }

    private void UpdateInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        didJump = Input.GetButtonDown("Jump");
        didDash = Input.GetMouseButtonDown(0);
    }

    private PState UpdateState()
    {
        if (currentState == PState.Dead) return PState.Dead;
        if (didDash && pf.CanDash()) return PState.Dash;
        if (didJump && pf.CanJump(moveInput)) return PState.Jump;
        if (pf.IsFalling() && !pf.MovingIntoWall(moveInput)) return PState.Fall;
        if (moveInput != 0)
        {
            if (pf.MovingIntoWall(moveInput))
            {
                wallPityTimer.Reset();
                return PState.Wall;
            }
            if (!wallPityTimer.isOver) return PState.Wall;
            if (pf.CanWalk()) return PState.Walk;
        }
        if (!wallPityTimer.isOver) return PState.Wall;
        return PState.Idle;
    }

    private void IdleState()
    {
        movement.OnIdle();
        // Animate
    }

    private void WalkState()
    {
        movement.OnMove(moveInput);
        // Animate
    }

    private void JumpState()
    {
        if (pf.MovingIntoWall(moveInput) && !pf.IsGrounded())
            jump.OnWallJump(moveInput);
        else jump.OnJump(moveInput);
        // animate
    }

    private void FallState()
    {
        jump.OnFall(moveInput);
        // animate
    }

    private void WallState()
    {
        jump.OnWallSlide();
        // animate
    }

    private void DashState()
    {
        dash.OnDash();
        pf.ResetDashTimer();
        // animate
    }

    private void DeadState()
    {
        // Respawn, animate
    }

}


/*
    On player collision, check for obstacle tag
    if player not dashing, die
    else check for bounceable/breakable and act accordingly
*/