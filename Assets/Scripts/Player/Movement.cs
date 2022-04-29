using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;

    // Player Parameters
    [SerializeField] private float forceScale;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float stopSpeed;
    [SerializeField] private float acceleration;

    private float maxActiveSpeed = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        maxMoveSpeed *= forceScale;
        acceleration *= forceScale;
    }

    public void Idle()
    {
        Vector2 vel = rb.velocity;
        vel.x = Mathf.MoveTowards(vel.x, 0, stopSpeed * Time.deltaTime);
        rb.velocity = vel;
    }

    public void Move(float direction)
    {
        float targetSpeed = direction;

        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            maxActiveSpeed = Mathf.Abs(rb.velocity.x);
            targetSpeed *= maxActiveSpeed;
        }
        else targetSpeed *= maxMoveSpeed;

        if (direction * rb.velocity.x < 0) Idle();
        else
        {
            float diffFromMax = targetSpeed - rb.velocity.x;
            rb.AddForce(Vector2.right * diffFromMax * Time.deltaTime);
        }

    }
}
