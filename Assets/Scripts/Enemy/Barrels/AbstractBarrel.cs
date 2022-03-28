using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBarrel : MonoBehaviour
{
    // For bullet management
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject bulletParentPrefab;
    [SerializeField] protected int initialBulletCount;

    // For shooting management
    [SerializeField] protected float shotCooldown;
    [SerializeField] protected GameObject bulletSpawnPoint;
    protected Timer shotTimer;
    public abstract void Shoot();
}
