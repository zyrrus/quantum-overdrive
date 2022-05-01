using UnityEngine;

public class DashState : PlayerBaseState
{
    public DashState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        context.IsDashing = true;
        context.DashEffectiveTimer.Reset();
        context.DashCooldownTimer.Reset();
        HandleDash();
    }

    public override void UpdateState()
    {
        if (!context.DashEffectiveTimer.isOver)
            context.DashEffectiveTimer.Tick();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        context.IsDashing = false;
        if (context.IsDashPressed) context.RequireNewDashPress = true;
    }

    public override void CheckSwitchStates()
    {
        if (!context.DashEffectiveTimer.isOver) return;

        if (context.IsMovementPressed) SwitchState(factory.Run());
        else SwitchState(factory.Idle());
    }

    public override void InitSubState() { }

    private void HandleDash()
    {
        Vector2 oldVel = context.Rb.velocity.normalized;
        context.Rb.velocity = new Vector2();

        bool dashForward = context.DashInput.magnitude == 0;

        if (dashForward)
            if (oldVel.magnitude < 0.1f)
                context.Rb.AddForce(oldVel * context.DashForce, ForceMode2D.Impulse);
            else
                context.Rb.AddForce(Vector2.right * context.FacingDirection * context.DashForce, ForceMode2D.Impulse);
        else
            context.Rb.AddForce(context.DashInput * context.DashForce, ForceMode2D.Impulse);
    }
}
