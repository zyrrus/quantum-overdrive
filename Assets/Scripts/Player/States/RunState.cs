using UnityEngine;

public class RunState : PlayerBaseState
{
    public RunState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        context.Animator.SetBool("Run", true);
    }

    public override void UpdateState()
    {
        HandleRun();
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.Animator.SetBool("Run", false);
    }

    public override void CheckSwitchStates()
    {
        if (context.IsFalling && context.IsTouchingWall) SwitchState(factory.WallSlide());
        else if (context.IsDashPressed && !context.RequireNewDashPress && context.DashCooldownTimer.isOver)
            SwitchState(factory.Dash());
        else if (!context.IsMovementPressed) SwitchState(factory.Idle());
    }

    public override void InitSubState() { }

    private void HandleRun()
    {
        float curXVel = context.Rb.velocity.x;

        // Stop player if changing direction
        if (curXVel * context.MovementInput < 0)
            context.ReduceXVelocity(context.StoppingSpeed * Time.deltaTime);

        // Apply acceleration if slower than MaxSpeed
        if (Mathf.Abs(curXVel) < context.SoftMaxSpeed)
            context.Rb.AddForce(Vector2.right * context.Acceleration * context.MovementInput * Time.deltaTime);
        // else Debug.Log("Top speed reached");
    }
}
