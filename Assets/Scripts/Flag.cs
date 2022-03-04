using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private LayerMask mask;

    public bool IsTriggered()
    {
        return Physics2D.OverlapPoint(transform.position, mask);
    }
}
