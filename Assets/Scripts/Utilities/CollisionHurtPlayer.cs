using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHurtPlayer : MonoBehaviour
{
    [SerializeField] private float damage = 10;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision Hit something " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.LoseHealth(damage);
        }
    }
}
