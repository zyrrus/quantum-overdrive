using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCannon : MonoBehaviour
{
    public Vector3 targetPosition { get; set; }
    private Vector3 currPosition ;
    [SerializeField] float rotateSpeed;

    private void Awake() {
        currPosition = transform.position + new Vector3(-1, 0, 0);
    }

    public void Aim() {
        // Debug.DrawRay(transform.position, Vector3.right, Color.red);
        // Debug.DrawRay(transform.position, targetPosition - transform.position, Color.red);
        // Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);

        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
}
