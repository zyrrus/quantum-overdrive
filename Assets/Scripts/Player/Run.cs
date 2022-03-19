using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
    }

    private void Update()
    {
        float targetSpeed = pc.inputTarget.x * maxSpeed;
        float diffFromMax = targetSpeed - rb.velocity.x;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * accel, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

}
