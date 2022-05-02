using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCollision : MonoBehaviour
{
    private PlayerStateMachine psm;

    private void Awake() => psm = GetComponent<PlayerStateMachine>();

    private void MoveRespawnTo(Vector3 newPos) => psm.RespawnPoint.position = newPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CheckPoint")
        {
            // Move respawn point to checkpoint
            CheckPoint point = other.gameObject.GetComponent<CheckPoint>();
            if (point.isNew)
            {
                MoveRespawnTo(other.gameObject.transform.position);
                point.DisableCheckPoint();
            }
        }
    }
}
