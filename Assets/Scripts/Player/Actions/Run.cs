using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [Header("Basic Movement")]
    [SerializeField] private float[] maxSpeed = new float[3];
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

    [Header("Speed Tiers")]
    [SerializeField] private float tierStallTime;
    private Timer tierTimer;
    private bool isStalling = false;
    private int speedTier = 0;


    public PlayerLog pl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
        pl = GetComponent<PlayerLog>();
    }

    private void Start() => tierTimer = new Timer(tierStallTime);

    private void Update()
    {
        UpdatedSpeedTier();

        float targetSpeed = pc.inputTarget.x * maxSpeed[speedTier];
        float diffFromMax = targetSpeed - rb.velocity.x;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * accel, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

    private void UpdatedSpeedTier()
    {
        pl.Log($"Speed tier: {speedTier}");

        isStalling = Mathf.Abs(rb.velocity.x - maxSpeed[speedTier]) < 0.1f;

        if (isStalling) tierTimer.Tick();
        else tierTimer.Reset();

        if (isStalling && tierTimer.isOver) speedTier++;

    }
}
