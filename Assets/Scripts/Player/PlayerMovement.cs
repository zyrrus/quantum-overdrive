using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private PlayerStats stats;

    // Movement
    private float inputDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        // ...
        Movement();
    }

    private void Movement()
    {
        rb.AddForce(transform.right * inputDir * stats.moveSpeed, ForceMode2D.Force);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<float>();
    }

    public void OnJump() { }
}
