using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector2 direction;
    [SerializeField] protected float damage;
    public abstract void setDirection(Vector3 dir);
    protected abstract void Move();
}
