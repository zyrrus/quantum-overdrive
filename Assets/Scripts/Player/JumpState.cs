using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory)
    {
        isRootState = true;
    }

    public override void EnterState()
    {

        InitSubState();
        Debug.Log("Jump");
        HandleJump();
    }

    public override void UpdateState() => CheckSwitchStates();
    public override void ExitState()
    {
        context.IsJumping = false;
        if (context.IsJumpPressed) context.RequireNewJumpPress = true;
    }


    public override void CheckSwitchStates()
    {
        if (context.IsGrounded) SwitchState(factory.Grounded());
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

        float maxJumpForce = context.JumpForce * context.JumpForceBonus;

        float slope = (context.JumpForce - maxJumpForce) / (context.SoftMaxSpeed - context.BonusMaxSpeed);
        float jumpScale = slope * (context.Rb.velocity.x - context.SoftMaxSpeed) + context.JumpForce;

        float scaledJumpForce = Mathf.Clamp(jumpScale, context.JumpForce, maxJumpForce);

        Debug.Log("scaled jump force: " + scaledJumpForce);
        context.Rb.AddForce(Vector2.up * scaledJumpForce, ForceMode2D.Impulse);
    }
}