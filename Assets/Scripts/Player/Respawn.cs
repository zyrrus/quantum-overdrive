using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private GameObject player;
    private PlayerStateMachine psm;
    private LevelManager lm;

    private void Start() {
        player = gameObject;
        psm = player.GetComponent<PlayerStateMachine>();
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "CheckPoint") {
            // Move respawn point to checkpoint
            CheckPoint point = other.gameObject.GetComponent<CheckPoint>();
            if (point.isNew) {
                respawnPoint.position = other.gameObject.transform.position;
                point.DisableCheckPoint();
            }
        }
        else if (other.tag == "Obstacle") {
            if (other.gameObject.GetComponent<ObstacleBase>().isDeadly) {
                // Move player to respawn point
                psm.KillXVelocity();
                psm.KillYVelocity();
                player.transform.position = respawnPoint.position;

                // Reset level obstacles
                lm.ResetLevel();
            }
        }
    }
}
