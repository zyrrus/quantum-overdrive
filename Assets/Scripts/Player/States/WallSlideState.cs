using UnityEngine;

public class WallSlideState : PlayerBaseState
{
    public WallSlideState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        context.KillYVelocity();
        context.Rb.gravityScale = context.WallSlideGravityScale;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        context.Rb.gravityScale = context.OriginalGravityScale;

        if (context.IsJumping || (context.IsJumpPressed && !context.RequireNewJumpPress))
            JumpAwayFromWall();
    }
    public override void CheckSwitchStates()
    {
        if (!context.IsMovementPressed) SwitchState(factory.Idle());
        else if (context.IsJumping || (context.IsJumpPressed && !context.RequireNewJumpPress))
            SwitchSuperState(factory.Jump());
    }

    public override void InitSubState() { }

    private void JumpAwayFromWall()
    {
        float ejectForce = -context.FacingDirection * 15;
        context.Rb.AddForce(Vector2.right * ejectForce, ForceMode2D.Impulse);
    }
}