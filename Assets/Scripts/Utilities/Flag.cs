using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    public bool IsTriggered()
    {
        return Physics2D.OverlapPoint(transform.position, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTriggered() ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
