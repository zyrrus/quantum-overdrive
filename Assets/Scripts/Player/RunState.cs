using UnityEngine;

public class RunState : PlayerBaseState
{
    public RunState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        Debug.Log("Run");
    }

    public override void UpdateState()
    {
        HandleRun();
        CheckSwitchStates();
    }
    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (!context.IsMovementPressed) SwitchState(factory.Idle());
    }

    public override void InitSubState() { }

    private void HandleRun()
    {
        float curXVel = context.Rb.velocity.x;
        if (curXVel * context.MovementInput < 0)
            context.Rb.AddForce(Vector2.right * (context.Acceleration / 2) * context.MovementInput * Time.deltaTime, ForceMode2D.Impulse);

        if (Mathf.Abs(curXVel) < context.SoftMaxSpeed)
            context.Rb.AddForce(Vector2.right * context.Acceleration * context.MovementInput * Time.deltaTime);
        else Debug.Log("Top speed reached");
    }
}
