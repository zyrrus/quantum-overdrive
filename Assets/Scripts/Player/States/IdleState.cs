using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        if (context.Rb.velocity.magnitude > 0)
            context.ReduceXVelocity(context.StoppingSpeed * Time.deltaTime);
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsDashPressed && !context.RequireNewDashPress && context.DashCooldownTimer.isOver)
            SwitchState(factory.Dash());
        else if (context.IsMovementPressed) SwitchState(factory.Run());
    }

    public override void InitSubState() { }
}
