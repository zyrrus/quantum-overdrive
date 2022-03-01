using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody rb;
    private PlayerStats stats;

    // Movement
    private float facing = 0;
    private float modelFacing = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        // ...
        Movement();
    }

    private void Movement()
    {
        if (facing * modelFacing < 0)
        {
            Debug.Log("flip");
        }

        rb.AddForce(new Vector3(facing * stats.moveSpeed * Time.deltaTime, 0, 0));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        facing = context.ReadValue<float>();
    }

    public void OnJump() { }
}
