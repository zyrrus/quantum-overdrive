using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurtPlayer : MonoBehaviour
{
    [SerializeField] private float damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Hit something " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.LoseHealth(damage);
        }
    }
}
