using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatePath : MonoBehaviour
{
    private ObstacleBase ob;
    [SerializeField] private Transform nodeA;
    [SerializeField] private Transform nodeB;
    [SerializeField] private Transform flipRoot;
    private bool isTargetingA;
    private Transform targetNode;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float stunTime;

    private Timer stunTimer;


    private void Awake()
    {
        ob = GetComponent<ObstacleBase>();
        stunTimer = new Timer(stunTime);
        targetNode = nodeA;
    }

    private void Update()
    {
        if (!stunTimer.isOver) stunTimer.Tick();
        if (ob.IsHit)
        {
            stunTimer.Reset();
            ob.IsHit = false;
        }

        if (stunTimer.isOver)
        {
            Vector3 deltaPos = transform.position - targetNode.position;
            if (deltaPos.magnitude < 0.1f)
            {
                targetNode = isTargetingA ? nodeB : nodeA;
                isTargetingA = !isTargetingA;
                Flip();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, moveSpeed * Time.deltaTime);
        }

    }

    private void Flip()
    {
        Vector3 scale = flipRoot.localScale;
        scale.x *= -1;
        flipRoot.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(nodeA.position, 0.3f);
        Gizmos.DrawWireSphere(nodeB.position, 0.3f);
    }
}
