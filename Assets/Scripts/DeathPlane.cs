using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" && other.gameObject.transform.position.y < gameObject.transform.position.y) {
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}
