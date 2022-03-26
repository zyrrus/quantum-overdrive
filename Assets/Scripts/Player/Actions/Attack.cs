using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private PlayerCore pc;
    [SerializeField] private ToggleHitboxes toggleHitboxes;

    [SerializeField] private float attackDuration;
    private Timer attackTimer;

    private void Awake()
    {
        pc = GetComponent<PlayerCore>();
    }

    private void Start()
    {
        attackTimer = new Timer(attackDuration);
    }

    private void Update()
    {
        if (attackTimer.isOver) toggleHitboxes.DisableHitbox();
        else attackTimer.Tick();
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float verticalThreshold = 1 / Mathf.Sqrt(2);

        AttackDirection attack = AttackDirection.Forward;

        if (pc.inputTarget.y >= verticalThreshold)
            attack = AttackDirection.Up;
        else if (pc.inputTarget.y <= -verticalThreshold)
            attack = AttackDirection.Down;

        toggleHitboxes.EnableHitbox(attack);
        attackTimer.Reset();
    }
}
