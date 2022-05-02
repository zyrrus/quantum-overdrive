using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    private Vector3 resetPoint;
    private bool isBouncy;

    private void Start() => resetPoint = gameObject.transform.position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerCollision>();
            // unfinished
        }
    }

    public void ResetObstacle() => gameObject.transform.position = resetPoint;
}
