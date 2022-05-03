public abstract class PlayerBaseState
{
    protected PlayerStateMachine context;
    protected PlayerStateFactory factory;
    protected PlayerBaseState currentSuperState;
    protected PlayerBaseState currentSubState;
    protected bool isRootState = false;

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        this.context = context;
        this.factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (currentSubState != null)
            currentSubState.UpdateStates();
    }

    private void EnterStates()
    {
        EnterState();
        if (currentSubState != null)
            currentSubState.EnterStates();
    }

    private void ExitStates()
    {
        ExitState();
        if (currentSubState != null)
            currentSubState.ExitStates();
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitStates();
        newState.EnterStates();

        if (isRootState)
            context.CurrentState = newState;
        else if (currentSuperState != null)
            currentSuperState.SetSubState(newState);
    }

    protected void SwitchSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState.SwitchState(newSuperState);
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    public string Name()
    {
        if (currentSubState != null)
            return $"{ToString()}: {currentSubState.Name()}";
        return ToString();
    }
}