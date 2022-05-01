using System.Collections.Generic;

enum PlayerStates { Grounded, Jump, Fall, Rise, Idle, Run, Dash, WallSlide }

public class PlayerStateFactory
{
    private readonly PlayerStateMachine context;
    Dictionary<PlayerStates, PlayerBaseState> states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine context)
    {
        this.context = context;
        states[PlayerStates.Grounded] = new GroundedState(context, this);
        states[PlayerStates.Jump] = new JumpState(context, this);
        states[PlayerStates.Fall] = new FallState(context, this);
        states[PlayerStates.Rise] = new RiseState(context, this);
        states[PlayerStates.Idle] = new IdleState(context, this);
        states[PlayerStates.Run] = new RunState(context, this);
        states[PlayerStates.Dash] = new DashState(context, this);
        states[PlayerStates.WallSlide] = new WallSlideState(context, this);
    }


    public PlayerBaseState Grounded() => states[PlayerStates.Grounded];
    public PlayerBaseState Jump() => states[PlayerStates.Jump];
    public PlayerBaseState Fall() => states[PlayerStates.Fall];
    public PlayerBaseState Rise() => states[PlayerStates.Rise];
    public PlayerBaseState Idle() => states[PlayerStates.Idle];
    public PlayerBaseState Run() => states[PlayerStates.Run];
    public PlayerBaseState Dash() => states[PlayerStates.Dash];
    public PlayerBaseState WallSlide() => states[PlayerStates.WallSlide];
}
