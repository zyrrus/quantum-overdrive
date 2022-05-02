using UnityEngine;

public class WallSlideState : PlayerBaseState
{
    public WallSlideState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        context.Rb.gravityScale = context.WallSlideGravityScale;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        context.Rb.gravityScale = context.OriginalGravityScale;
    }
    public override void CheckSwitchStates() { }

    public override void InitSubState() { }
}