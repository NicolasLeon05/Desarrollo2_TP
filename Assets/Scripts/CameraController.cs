using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform pivot;
    [SerializeField] private InputActionReference moveCamera;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float pitchMin = -40f;
    [SerializeField] private float pitchMax = 70f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (moveCamera != null)
        {
            moveCamera.action.performed += OnMoveCamera;
            moveCamera.action.canceled += OnMoveCamera;
        }
    }

    private void OnMoveCamera(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * sensitivity;
        pitch += lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        target.rotation = Quaternion.Euler(0f, yaw, 0f);
        pivot.localRotation = Quaternion.Euler(-pitch, 0f, 0f);

       transform.position = pivot.position + pivot.rotation * offset;
        transform.LookAt(pivot.position);
    }

}
