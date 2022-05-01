using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float dashForce;
    [SerializeField] private float dashEffectiveTime;
    private Timer dashEffectiveTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dashEffectiveTimer = new Timer(dashEffectiveTime);
    }

    private void Update()
    {
        if (!dashEffectiveTimer.isOver) dashEffectiveTimer.Tick();
    }

    public bool IsDashing() => !dashEffectiveTimer.isOver;

    public void OnDash()
    {
        Vector2 dir = new Vector2(Mathf.Sign(rb.velocity.x), 0);
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    }
}
