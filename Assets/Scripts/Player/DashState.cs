using UnityEngine;

public class DashState : PlayerBaseState
{
    public DashState(PlayerStateMachine context, PlayerStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        Debug.Log("Dash");
    }

    public override void UpdateState() { }
    public override void ExitState() { }
    public override void CheckSwitchStates() { }
    public override void InitSubState() { }
}
