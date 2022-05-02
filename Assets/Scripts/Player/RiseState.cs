using UnityEngine;

public class RiseState : PlayerBaseState
{
    public RiseState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) => isRootState = true;

    public override void EnterState() => InitSubState();

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsGrounded) SwitchState(factory.Grounded());
        else if (context.IsFalling) SwitchState(factory.Fall());
    }

    public override void InitSubState()
    {
        if (context.IsDashPressed) SetSubState(factory.Dash());
        else if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }
}