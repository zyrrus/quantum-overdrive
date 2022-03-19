using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Shader")]
    [SerializeField] private Shader globalShader;

    [Header("Smooth Camera")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private float smoothingFactor;
    [SerializeField] private float lookRadius;
    [SerializeField] private Vector3 initOffset = new Vector3(0, 0.5f, -35);

    private Vector2 targetPos;

    private void LateUpdate()
    {
        Vector3 desired = new Vector3(targetPos.x + transform.position.x, targetPos.y + transform.position.y, 0);
        desired += initOffset;

        playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, desired, smoothingFactor * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        targetPos = context.ReadValue<Vector2>() * lookRadius;
    }
}
