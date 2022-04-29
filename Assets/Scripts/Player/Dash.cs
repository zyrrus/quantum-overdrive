using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float dashForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnDash()
    {
        Vector2 dir = new Vector2(Mathf.Sign(rb.velocity.x), 0);
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    }
}
