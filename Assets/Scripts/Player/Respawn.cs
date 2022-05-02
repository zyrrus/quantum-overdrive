using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private GameObject player;
    private PlayerStateMachine psm;
    private LevelManager lm;

    private void Start()
    {
        player = gameObject;
        psm = player.GetComponent<PlayerStateMachine>();
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CheckPoint")
        {
            // Move respawn point to checkpoint
            CheckPoint point = other.gameObject.GetComponent<CheckPoint>();
            if (point.isNew)
            {
                respawnPoint.position = other.gameObject.transform.position;
                point.DisableCheckPoint();
            }
        }
    }
}
