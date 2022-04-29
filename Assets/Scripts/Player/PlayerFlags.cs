using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlags : MonoBehaviour
{
    private Vector3 leftCorner;
    private Vector3 rightCorner;

    private void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        leftCorner = collider.bounds.min;
        rightCorner = leftCorner + (Vector3.right * collider.bounds.size.x);
    }

    public bool IsGrounded()
    {
        bool hitLeft = Physics2D.Raycast(leftCorner, Vector2.down, 0.03f).collider != null;
        bool hitRight = Physics2D.Raycast(rightCorner, Vector2.down, 0.03f).collider != null;
        return hitLeft || hitRight;
    }

    public bool IsFalling()
    {
        return false;
    }

    public bool CanDash()
    {
        return true;
    }

    public bool CanJump()
    {
        return true;
    }

    public bool MovingIntoWall(float direction)
    {
        return true;
    }

    public bool CanWalk()
    {
        return true;
    }


}
