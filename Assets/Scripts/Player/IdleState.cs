using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        Debug.Log("Idle");
    }

    public override void UpdateState()
    {
        if (context.Rb.velocity.magnitude > 0)
        {
            Vector2 vel = context.Rb.velocity;
            vel.x = Mathf.MoveTowards(vel.x, 0, context.StoppingSpeed * Time.deltaTime);
            context.Rb.velocity = vel;
        }
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (context.IsMovementPressed) SwitchState(factory.Run());
    }

    public override void InitSubState() { }
}
