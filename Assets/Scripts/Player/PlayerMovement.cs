using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private PlayerInput input;
    private PlayerStats stats;

    // Physics
    private float curVelocity;
    private float curAcceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        Jump();
        Movement();
    }

    private void Movement()
    {
        rb.AddForce(transform.right * input.inputDir * stats.moveSpeed, ForceMode2D.Force);
    }

    private void Jump()
    {
        if (input.GetPressedJump())
            rb.AddForce(transform.up * stats.jumpStrength, ForceMode2D.Impulse);
    }
}
