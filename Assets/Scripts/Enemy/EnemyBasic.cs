using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : StateMachineEnemyBasic
{
    float moveSpeed;
    int attackDamage;
    int hp;

    Vector3 homeLocation;

    Transform player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
        homeLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void DetectAction() {
        Debug.Log("DetectAction");
        // idle
    }

    public override void TrackAction() {
        Debug.Log("TrackAction");
        // move towards player
    }

    public override void AttackAction() {
        Debug.Log("AttackAction");
    }

    public override void ResetAction() {
        Debug.Log("ResetAction");
    }
}
