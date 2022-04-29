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

    private bool isFacingRight = true;


    private void Awake()
    {
        pf = GetComponent<PlayerFlags>();
        movement = GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
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
        didJump = Input.GetAxisRaw("Jump") != 0;
        didDash = Input.GetMouseButtonDown(0);
    }

    private PState UpdateState()
    {
        if (currentState == PState.Dead) return PState.Dead;
        if (didDash && pf.CanDash()) return PState.Dash;
        if (didJump && pf.CanJump()) return PState.Jump;
        if (pf.IsFalling()) return PState.Fall;
        if (moveInput != 0)
        {
            if (pf.CanWalk())
                return PState.Walk;
            if (pf.MovingIntoWall(moveInput))
                return PState.Wall;
        }
        return PState.Idle;
    }

    private void IdleState()
    {
        movement.Idle();
        // Animate
    }

    private void WalkState()
    {
        movement.Move(moveInput);
        // Animate
    }

    private void JumpState()
    {
        // Movement.Jump(moveinput), animate
    }

    private void FallState()
    {
        // Movement.Fall(moveinput), animate
    }

    private void WallState()
    {
        // Movement.WallSlide(), animate
    }

    private void DashState()
    {
        // Dash.Dash(), animate
    }

    private void DeadState()
    {
        // Respawn, animate
    }

}
