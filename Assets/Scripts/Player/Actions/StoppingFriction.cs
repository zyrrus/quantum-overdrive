using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingFriction : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [SerializeField, Range(0, 1)] private float frictionStrength;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if (pc.isGrounded && Mathf.Abs(pc.inputTarget.x) < 0.01f)
        {
            float friction = Mathf.Min(Mathf.Abs(rb.velocity.x), frictionStrength);
            friction *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.left * friction, ForceMode2D.Impulse);
        }
    }
}
