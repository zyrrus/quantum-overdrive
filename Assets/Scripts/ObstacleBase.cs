using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    private Transform resetPoint;

    private void Start() {
        resetPoint = gameObject.transform;
    }

    public void ResetObstacle() {
        gameObject.transform.position = resetPoint.position;
    }
}
