using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : StateMachineEnemyBasic
{
    [SerializeField] TargetCannon cannon;
    Transform player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void DetectAction() {
        // Debug.Log("DetectCannon");
        // idle
    }

    public override void TrackAction() {
        // Static Enemy - No movement action
        AimCannon();
    }

    public override void AttackAction() {
        // Debug.Log("AttackCannon");
        AimCannon();
    }

    public override void ResetAction() {
        // Debug.Log("ResetCannon");
    }

    private void AimCannon() {
        // Aim at player
        cannon.targetPosition = player.position;
        cannon.Aim();
    }
}
