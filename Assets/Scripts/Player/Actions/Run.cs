using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;
    [SerializeField] private UILogger pl;

    [Header("Basic Movement")]
    [SerializeField] private float[] maxTargetSpeed = new float[3];
    private float maxSpeed;
    [SerializeField] private float[] acceleration = new float[3];
    [SerializeField] private float[] deceleration = new float[3];
    [SerializeField] private float velPower;

    [Header("Speed Tiers")]
    [SerializeField] private float tierStallTime;
    private Timer tierStallTimer;
    [SerializeField] private float tierDecayTime;
    private Timer tierDecayTimer;
    [SerializeField] private float tierStallResetTime;
    private Timer tierStallResetTimer;
    [SerializeField] private float maxVelocityChangeRate;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
        // pl = GetComponent<UILogger>();
    }

    private void Start()
    {
        tierStallTimer = new Timer(tierStallTime);
        tierDecayTimer = new Timer(tierDecayTime);
        tierStallResetTimer = new Timer(tierStallResetTime);

        maxSpeed = maxTargetSpeed[0];
    }

    private void Update()
    {
        UpdatedSpeedTier();

        float targetSpeed = pc.inputTarget.x * maxSpeed;
        float diffFromMax = targetSpeed - rb.velocity.x;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration[pc.speedTier] : deceleration[pc.speedTier];

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * accel, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

    private void UpdatedSpeedTier()
    {
        pl.ReplaceLog($"Speed tier: {pc.speedTier}");
        pl.PushLog($"\nVelocity: {rb.velocity.x}");

        float prevSpeedTier = maxTargetSpeed[Mathf.Clamp(pc.speedTier - 1, 0, 2)];

        bool isStalling = Mathf.Abs(rb.velocity.x) > maxTargetSpeed[pc.speedTier] - 0.25f;
        bool isLagging = Mathf.Abs(rb.velocity.x) < prevSpeedTier + 0.25f;

        updateStallTimer(isStalling);
        updateDecayTimer(isLagging);


        if (pc.speedTier != 2 && isStalling && tierStallTimer.isOver)
        {
            tierStallTimer.Reset();
            pc.speedTier++;
            StartCoroutine(LerpMaxSpeed(prevSpeedTier));
        }

        else if (pc.speedTier != 0 && isLagging && tierDecayTimer.isOver)
        {
            tierDecayTimer.Reset();
            pc.speedTier--;
            StartCoroutine(LerpMaxSpeed(maxTargetSpeed[Mathf.Clamp(pc.speedTier + 1, 0, 2)]));
        }

        pl.PushLog("\nStall " + tierStallTimer.ToString());
        pl.PushLog("\nDecay " + tierDecayTimer.ToString());
        pl.PushLog("\nReset " + tierStallResetTimer.ToString());
        pl.PushLog("\nMaxSpeed " + maxSpeed);
    }

    private void updateStallTimer(bool isStalling)
    {
        if (isStalling)
        {
            tierStallResetTimer.Reset();
            tierStallTimer.Tick();
        }
        else
        {
            tierStallResetTimer.Tick();
            if (tierStallResetTimer.isOver)
                tierStallTimer.Reset();
        }
    }

    private void updateDecayTimer(bool isLagging)
    {
        if (isLagging) tierDecayTimer.Tick();
        else tierDecayTimer.Reset();
    }

    private IEnumerator LerpMaxSpeed(float prevSpeed)
    {
        float targetSpeed = maxTargetSpeed[pc.speedTier];
        float t = 0;
        while (t < 1)
        {
            maxSpeed = Mathf.Lerp(prevSpeed, targetSpeed, t);

            t += Time.deltaTime / maxVelocityChangeRate;

            yield return null;
        }

        maxSpeed = targetSpeed;
    }
}
