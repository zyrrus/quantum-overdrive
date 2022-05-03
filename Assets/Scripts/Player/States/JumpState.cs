using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) => isRootState = true;

    public override void EnterState()
    {
        InitSubState();
        context.Animator.SetBool("Jump", true);
        HandleJump();
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState()
    {
        if (context.IsJumpPressed) context.RequireNewJumpPress = true;
        context.Animator.SetBool("Jump", false);
    }


    public override void CheckSwitchStates()
    {
        if (!context.IsJumping && context.IsGrounded) SwitchState(factory.Grounded());
        else if (context.IsFalling) SwitchState(factory.Fall());
    }

    public override void InitSubState()
    {
        if (!context.IsMovementPressed) SetSubState(factory.Idle());
        else SetSubState(factory.Run());
    }

    private void HandleJump()
    {
        context.IsJumping = true;

        context.KillYVelocity();

        float maxJumpForce = context.JumpForce * context.JumpForceBonus;

        float scaledJumpForce = (context.Rb.velocity.x > context.BonusMaxSpeed) ? maxJumpForce : context.JumpForce;

        context.Rb.AddForce(Vector2.up * scaledJumpForce, ForceMode2D.Impulse);
    }
}