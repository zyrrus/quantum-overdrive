using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private float damage = 5;

    public void SetDamage(float amount) => damage = amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy") return;

        Health enemyHP = other.GetComponent<Health>();
        enemyHP.LoseHealth(damage);
    }
}
