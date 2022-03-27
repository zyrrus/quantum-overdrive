using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private readonly float verticalThreshold = 1 / Mathf.Sqrt(2);

    private PlayerCore pc;
    [SerializeField] private ToggleHitboxes toggleHitboxes;

    [SerializeField] private float lightAttackDuration;
    [SerializeField] private float heavyAttackDuration;
    private Timer attackTimer;

    [SerializeField] private float lightAttackDamage;
    [SerializeField] private float heavyAttackDamage;
    [SerializeField] private float[] speedDamageMultiplier = { 0.25f, 1f, 1.5f };

    private void Awake() => pc = GetComponent<PlayerCore>();
    private void Start() => attackTimer = new Timer(0);

    private void Update()
    {
        if (attackTimer.isOver) toggleHitboxes.DisableHitbox();
        else attackTimer.Tick();
    }

    public void OnLightAttack(InputAction.CallbackContext context) => OnAttack(context, lightAttackDamage, lightAttackDuration);
    public void OnHeavyAttack(InputAction.CallbackContext context) => OnAttack(context, heavyAttackDamage, heavyAttackDuration);
    private void OnAttack(InputAction.CallbackContext context, float baseDamage, float duration)
    {
        if (!context.performed) return;
        if (!attackTimer.isOver) return;

        AttackDirection attack = GetAttackDirection();

        toggleHitboxes.EnableHitbox(attack, baseDamage * speedDamageMultiplier[pc.speedTier]);
        attackTimer.Reset(duration);
    }

    private AttackDirection GetAttackDirection()
    {
        AttackDirection attack = AttackDirection.Forward;

        if (pc.inputTarget.y >= verticalThreshold)
            attack = AttackDirection.Up;
        else if (pc.inputTarget.y <= -verticalThreshold)
            attack = AttackDirection.Down;

        return attack;
    }
}
