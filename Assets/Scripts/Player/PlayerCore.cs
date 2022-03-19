using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Flag groundFlag;
    [SerializeField] private Flag upperWallFlag;
    [SerializeField] private Flag lowerWallFlag;
    [SerializeField] private float rotationDuration = 0.1f;

    public Vector2 inputTarget { get; private set; }
    private Vector2 lastTarget = new Vector2(1, 0);
    public bool isFacingRight { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isHittingLowerWall { get; private set; }
    public bool isHittingUpperWall { get; private set; }

    private void Start()
    {
        isFacingRight = true;
    }

    private void Update()
    {
        isGrounded = groundFlag.IsTriggered();
        isHittingUpperWall = upperWallFlag.IsTriggered();
        isHittingLowerWall = lowerWallFlag.IsTriggered();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (inputTarget.x != 0) lastTarget.x = inputTarget.x;
        if (inputTarget.y != 0) lastTarget.y = inputTarget.y;

        inputTarget = context.ReadValue<Vector2>();

        if (IsFacingWrongWay()) FlipCharacter();
    }

    private bool IsFacingWrongWay() => lastTarget.x * inputTarget.x < 0;
    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        StartCoroutine(FlipCharLerp());
    }

    private IEnumerator FlipCharLerp()
    {
        int facingSign = (isFacingRight) ? 1 : -1;

        Quaternion endGoal = Quaternion.LookRotation(Vector3.forward * facingSign, Vector3.up);

        float timeElapsed = 0;
        while (timeElapsed < rotationDuration)
        {
            Vector3 direction = Vector3.Slerp(Vector3.forward, -Vector3.forward, timeElapsed / rotationDuration);
            Quaternion rotation = Quaternion.LookRotation(direction * -facingSign, Vector3.up);

            transform.rotation = rotation;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = endGoal;
    }

    private void OnDrawGizmos()
    {
        float rad = 2;
        Vector2 root = transform.position;

        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(root + (inputTarget * rad), 0.1f);
    }
}
