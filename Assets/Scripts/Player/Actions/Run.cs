using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;
    private UILogger pl;

    [Header("Basic Movement")]
    [SerializeField] private float[] maxSpeed = new float[3];
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

    [Header("Speed Tiers")]
    [SerializeField] private float tierStallTime;
    private Timer tierStallTimer;
    [SerializeField] private float tierDecayTime;
    private Timer tierDecayTimer;
    private bool isStalling = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
        pl = GetComponent<UILogger>();
    }

    private void Start()
    {
        tierStallTimer = new Timer(tierStallTime);
        tierDecayTimer = new Timer(tierDecayTime);
    }

    private void Update()
    {
        UpdatedSpeedTier();

        float targetSpeed = pc.inputTarget.x * maxSpeed[pc.speedTier];
        float diffFromMax = targetSpeed - rb.velocity.x;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * accel, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

    private void UpdatedSpeedTier()
    {
        pl.ReplaceLog($"Speed tier: {pc.speedTier}");
        pl.PushLog($"\nVelocity: {rb.velocity.x}");

        isStalling = Mathf.Abs(rb.velocity.x - maxSpeed[pc.speedTier]) < 0.1f;

        if (isStalling) tierStallTimer.Tick();
        else tierStallTimer.Reset();

        if (isStalling && tierStallTimer.isOver && pc.speedTier < maxSpeed.Length - 1) pc.speedTier++;

    }
}
