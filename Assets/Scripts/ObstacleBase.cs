using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    private Transform resetPoint;
    [SerializeField] private bool _isBouncy;
    [SerializeField] private bool _isDeadly;
    public bool isBouncy { get => _isBouncy; private set {_isBouncy = value;} }
    public bool isDeadly { get => _isDeadly; private set {_isDeadly = value;} }

    private void Start() {
        resetPoint = gameObject.transform;
    }

    public void ResetObstacle() {
        gameObject.transform.position = resetPoint.position;
    }
}
