using UnityEngine;

public class RunState : PlayerBaseState
{
    public RunState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        HandleRun();
        CheckSwitchStates();
    }
    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsDashPressed && !context.RequireNewDashPress && context.DashCooldownTimer.isOver)
            SwitchState(factory.Dash());
        else if (!context.IsMovementPressed) SwitchState(factory.Idle());
    }

    public override void InitSubState() { }

    private void HandleRun()
    {
        float curXVel = context.Rb.velocity.x;

        // Stop player if changing direction
        if (curXVel * context.MovementInput < 0)
        {
            Vector2 vel = context.Rb.velocity;
            vel.x = Mathf.MoveTowards(vel.x, 0, context.StoppingSpeed * Time.deltaTime);
            context.Rb.velocity = vel;
        }

        // Apply acceleration if slower than MaxSpeed
        if (Mathf.Abs(curXVel) < context.SoftMaxSpeed)
            context.Rb.AddForce(Vector2.right * context.Acceleration * context.MovementInput * Time.deltaTime);
        else Debug.Log("Top speed reached");
    }
}
