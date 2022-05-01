using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;

    // Player Parameters
    [SerializeField] private float _maxMoveSpeed;
    public float maxMoveSpeed { get => _maxMoveSpeed; private set { _maxMoveSpeed = value; } }
    [SerializeField] private float stopSpeed;
    [SerializeField] private float acceleration;

    private float maxActiveSpeed = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnIdle()
    {
        Vector2 vel = rb.velocity;
        vel.x = Mathf.MoveTowards(vel.x, 0, stopSpeed * Time.deltaTime);
        rb.velocity = vel;
    }

    public void OnMove(float direction)
    {
        float targetSpeed = direction;

        // Accelerate up to max speed, but maintain 
        // velocity if higher than max
        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            maxActiveSpeed = Mathf.Abs(rb.velocity.x);
            targetSpeed *= maxActiveSpeed;
        }
        else targetSpeed *= maxMoveSpeed;

        // When changing directions, idle until stopped
        if (direction * rb.velocity.x < 0) OnIdle();
        else
        {
            float accel = (Mathf.Abs(rb.velocity.x) >= Mathf.Abs(targetSpeed)) ? 0 : acceleration * Mathf.Sign(targetSpeed);
            rb.AddForce(Vector2.right * accel * Time.deltaTime);
        }
    }

    public void OnAirMove(float direction)
    {
        float targetSpeed = direction;

        // Accelerate up to max speed, but maintain 
        // velocity if higher than max
        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            maxActiveSpeed = Mathf.Abs(rb.velocity.x);
            targetSpeed *= maxActiveSpeed;
        }
        else targetSpeed *= maxMoveSpeed;

        float accel = (Mathf.Abs(rb.velocity.x) >= Mathf.Abs(targetSpeed)) ? 0 : acceleration * Mathf.Sign(targetSpeed);
        rb.AddForce(Vector2.right * accel * Time.deltaTime);

    }
}
