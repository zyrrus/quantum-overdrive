using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedEnemy : MonoBehaviour
{

    // TODO: Damage player on touch

    private Rigidbody2D rb;
    [SerializeField] private Transform flipParent;
    [SerializeField] private Flag forwardAFlag;
    [SerializeField] private Flag forwardBFlag;
    [SerializeField] private float rotationDuration;
    private bool isFacingRight = true;
    private bool isRotating = false;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float velPower;

    private void Awake() => rb = GetComponent<Rigidbody2D>();


    private void Update()
    {
        if (forwardAFlag.IsTriggered() || forwardBFlag.IsTriggered()) FlipModel();

        // Calculate movement force
        float targetSpeed = (isFacingRight ? 1 : -1) * maxSpeed;
        float diffFromMax = targetSpeed - rb.velocity.x;

        float moveForce = Mathf.Pow(Mathf.Abs(diffFromMax) * acceleration, velPower) * Mathf.Sign(diffFromMax);

        rb.AddForce(Vector2.right * moveForce);
    }

    private void FlipModel()
    {
        if (!isRotating)
        {
            isFacingRight = !isFacingRight;
            isRotating = true;
            StartCoroutine(FlipModelLerp());
        }
    }

    private IEnumerator FlipModelLerp()
    {
        int facingSign = (isFacingRight) ? 1 : -1;

        Quaternion endGoal = Quaternion.LookRotation(Vector3.forward * facingSign, Vector3.up);

        float timeElapsed = 0;
        while (timeElapsed < rotationDuration)
        {
            Vector3 direction = Vector3.Slerp(Vector3.forward, -Vector3.forward, timeElapsed / rotationDuration);
            Quaternion rotation = Quaternion.LookRotation(direction * -facingSign, Vector3.up);

            flipParent.rotation = rotation;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        flipParent.rotation = endGoal;
        isRotating = false;
    }
}
