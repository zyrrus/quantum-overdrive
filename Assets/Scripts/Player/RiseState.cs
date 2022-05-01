using UnityEngine;

public class RiseState : PlayerBaseState
{
    public RiseState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() { }
    public override void CheckSwitchStates() { }
    public override void InitSubState() { }
}