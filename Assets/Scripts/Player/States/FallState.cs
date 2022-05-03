using UnityEngine;

public class FallState : PlayerBaseState
{
    public FallState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) => isRootState = true;

    public override void EnterState()
    {
        InitSubState();
        context.IsJumping = false;
        context.Animator.SetBool("Falling", true);
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState()
    {
        context.Animator.SetBool("Falling", false);
    }

    public override void CheckSwitchStates()
    {
        if (context.IsGrounded) SwitchState(factory.Grounded());
        else if (context.IsDashPressed) SwitchState(factory.Rise());
    }

    public override void InitSubState()
    {
        if (context.IsDashing || (context.IsDashPressed && !context.RequireNewDashPress && context.DashCooldownTimer.isOver))
            SetSubState(factory.Dash());
        else if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }
}
