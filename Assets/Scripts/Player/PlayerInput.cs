using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // External

    public float inputDir { get; private set; }
    private bool pressedJump;

    // Internal 

    private PlayerStats stats;
    private CoyoteTimer jumpTimer;
    private bool isGrounded;

    // Getter/Setters

    public bool GetPressedJump()
    {
        // Reset once PlayerMovement knows 
        // that the player has jumped
        if (pressedJump)
        {
            pressedJump = false;
            return true;
        }

        return false;
    }

    // Unity Cycle

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        jumpTimer = new CoyoteTimer(stats.jumpCoyoteTime);
    }

    private void Update()
    {
        jumpTimer.Tick();
    }


    // Input Events

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        pressedJump = true;
        jumpTimer.Activate();
    }
}

class CoyoteTimer
{
    private float currentTime = 0;
    private float cooldownTime;

    public CoyoteTimer(float maxDuration)
    {
        cooldownTime = maxDuration;
    }

    public void Activate()
    {
        currentTime = cooldownTime;
    }

    public void Tick()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
    }

    public bool IsUsable()
    {
        return currentTime > 0;
    }
}