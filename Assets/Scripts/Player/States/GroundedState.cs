using UnityEngine;

public class GroundedState : PlayerBaseState
{
    public GroundedState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) => isRootState = true;

    public override void EnterState()
    {
        InitSubState();
        context.Animator.SetBool("Grounded", true);
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState()
    {
        context.Animator.SetBool("Grounded", false);
    }

    public override void CheckSwitchStates()
    {
        if (context.IsFalling) SwitchState(factory.Fall());
        else if (context.IsDashPressed) SwitchState(factory.Rise());
        else if (context.IsJumping || (context.IsJumpPressed && !context.RequireNewJumpPress))
            SwitchState(factory.Jump());
    }

    public override void InitSubState()
    {
        if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }
}
