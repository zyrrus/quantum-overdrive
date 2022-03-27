using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] StateMachineEnemyBasic parentScript;
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (gameObject.tag == "EnemyDetection") {
                // Debug.Log("Detect");
                parentScript.PlayerDetected = true;
            }
            else if (gameObject.tag == "EnemyTracking") {
                // Debug.Log("Track");
                parentScript.PlayerTracking = true;
            }
            else if (gameObject.tag == "EnemyAttacking") {
                // Debug.Log("Attack");
                parentScript.PlayerAttacking = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (gameObject.tag == "EnemyDetection") {
                // Debug.Log("Detect Off");
                parentScript.PlayerDetected = false;
            }
            else if (gameObject.tag == "EnemyTracking") {
                // Debug.Log("Track Off");
                parentScript.PlayerTracking = false;
            }
            else if (gameObject.tag == "EnemyAttacking") {
                // Debug.Log("Attack Off");
                parentScript.PlayerAttacking = false;
            }
        }
    }
}
