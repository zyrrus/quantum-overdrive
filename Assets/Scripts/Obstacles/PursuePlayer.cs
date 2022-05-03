using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private ObstacleBase ob;

    [SerializeField] private float detectionRadius;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stunTime;

    private Timer stunTimer;


    private void Awake()
    {
        ob = GetComponent<ObstacleBase>();
        stunTimer = new Timer(stunTime);
    }

    private void Update()
    {
        if (!stunTimer.isOver) stunTimer.Tick();
        if (ob.IsHit)
        {
            stunTimer.Reset();
            ob.IsHit = false;
        }

        Vector3 deltaPos = player.transform.position - transform.position;
        if (stunTimer.isOver && deltaPos.magnitude < detectionRadius)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
