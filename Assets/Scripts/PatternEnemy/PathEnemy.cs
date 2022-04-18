using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnemy : MonoBehaviour
{
    // TODO: Damage player on touch and 
    // slow down instead of stopping abruptly

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform transformParent;
    [SerializeField] private Transform pathParent;

    private Transform[] pathNodes;
    private bool reversePath = false;
    [SerializeField] private int targetNode = 1;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float velPower;

    private void Start()
    {
        pathNodes = pathParent.GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        // Calculate movement force
        Vector2 moveDir = (pathNodes[targetNode].position - transformParent.position);

        if (moveDir.magnitude < 0.1f)
        {
            NextNode();
            rb.velocity = new Vector2();
        }
        else
        {
            float diffFromMax = maxSpeed - rb.velocity.magnitude;
            if (diffFromMax < 0.1f) diffFromMax = 0;

            float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * acceleration, velPower);
            rb.AddForce(moveDir.normalized * moveForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transformParent.position, pathNodes[targetNode].position);
    }

    private void NextNode()
    {
        // 'Bounce' between node indices
        // ex: for [a, b, c, d], ignore a
        // -> b, c, d, c, b, c, d, ...

        if (pathNodes.Length <= 2) return;

        if (reversePath)
        {
            if (targetNode > 1) targetNode--;
            else
            {
                targetNode++;
                reversePath = false;
            }
        }
        else
        {
            if (targetNode < pathNodes.Length - 1) targetNode++;
            else
            {
                targetNode--;
                reversePath = true;
            }
        }
    }
}
