using UnityEngine;

public class FallState : PlayerBaseState
{
    public FallState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory)
    {
        isRootState = true;
    }

    public override void EnterState()
    {
        InitSubState();
        Debug.Log("Fall");
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsGrounded) SwitchState(factory.Grounded());
        else if (context.IsJumpPressed && !context.RequireNewJumpPress)
            SwitchState(factory.Jump());
    }

    public override void InitSubState()
    {
        if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }
}
