using UnityEngine;

public class GroundedState : PlayerBaseState
{
    public GroundedState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory)
    {
        isRootState = true;
    }

    public override void EnterState()
    {
        InitSubState();
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsFalling) SwitchState(factory.Fall());
        else if ((!context.IsGrounded && context.IsDashing)) SwitchState(factory.Rise());
        else if (context.IsJumping || (context.IsJumpPressed && !context.RequireNewJumpPress))
            SwitchState(factory.Jump());
    }

    public override void InitSubState()
    {
        if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }
}
