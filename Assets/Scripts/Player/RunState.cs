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
        if (context.MovementInput == 0) return;

        float curXVel = context.Rb.velocity.x;
        if (curXVel * context.MovementInput < 0)
            context.Rb.AddForce(Vector2.right * (context.Acceleration / 2) * context.MovementInput * Time.deltaTime, ForceMode2D.Impulse);

        if (Mathf.Abs(curXVel) < context.SoftMaxSpeed)
            context.Rb.AddForce(Vector2.right * context.Acceleration * context.MovementInput * Time.deltaTime);
        else Debug.Log("Top speed reached");

        if (curXVel == 0)
        {
            Debug.Log($"MoveInput: {context.MovementInput}\n" +
                      $"Cond1: {curXVel * context.MovementInput < 0}\n" +
                      $"Cond2: {Mathf.Abs(curXVel) < context.SoftMaxSpeed}");
        }
    }

    private void HandleRunOld()
    {
        float targetSpeed = context.MovementInput;

        // Accelerate up to max speed, but maintain 
        // velocity if higher than max
        if (Mathf.Abs(context.Rb.velocity.x) > context.SoftMaxSpeed)
        {
            context.CurrentMaxSpeed = Mathf.Abs(context.Rb.velocity.x);
            targetSpeed *= context.CurrentMaxSpeed;
        }
        else targetSpeed *= context.SoftMaxSpeed;

        // When changing directions, idle until stopped
        // if (context.MovementInput * context.Rb.velocity.x < 0)
        // {
        //     // OnIdle();
        // }
        // else
        {
            float accel = (Mathf.Abs(context.Rb.velocity.x) >= Mathf.Abs(targetSpeed))
                            ? 0 : context.Acceleration * Mathf.Sign(targetSpeed);
            context.Rb.AddForce(Vector2.right * accel * Time.deltaTime);
        }
    }
}
