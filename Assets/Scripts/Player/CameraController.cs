using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform pivot;
    [SerializeField] private InputActionReference moveCamera;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float controllerSensitivity = 100f;
    [SerializeField] private float pitchMin = -40f;
    [SerializeField] private float pitchMax = 70f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;
    private float currentSensitivity;

    /// <summary>
    /// Locks the cursor and subscribes to camera input events.
    /// Tries to assign the player and its pivot.
    /// Sets default sensitivity
    /// </summary>
    private void OnEnable()
    {
        GameManager.Instance.LockCursor();

        if (moveCamera != null)
        {
            moveCamera.action.performed += OnMoveCamera;
            moveCamera.action.canceled += OnMoveCamera;
        }

        TryAssignPlayerAndPivot();
        currentSensitivity = mouseSensitivity;
    }

    /// <summary>
    /// Unsubscribes from camera input events when disabled
    /// </summary>
    private void OnDisable()
    {
        if (moveCamera != null)
        {
            moveCamera.action.performed -= OnMoveCamera;
            moveCamera.action.canceled -= OnMoveCamera;
        }
    }

    /// <summary>
    /// Searches for the player and its Pivot transform if not already assigned
    /// </summary>
    private void TryAssignPlayerAndPivot()
    {
        if (target == null && Player.Instance != null)
            target = Player.Instance.transform;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (target != null && pivot == null)
        {
            Transform foundPivot = target.Find("Pivot");
            if (foundPivot != null)
                pivot = foundPivot;
        }
    }

    /// <summary>
    /// Reads camera input and stores it as look input.
    /// Sets the current sensitivity depending on the input device (mouse or gamepad)
    /// </summary>
    private void OnMoveCamera(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            lookInput = context.ReadValue<Vector2>();

            var device = context.control.device;
            if (device is Mouse)
                currentSensitivity = mouseSensitivity;
            else if (device is Gamepad)
                currentSensitivity = controllerSensitivity;
        }
    }

    /// <summary>
    /// Updates camera yaw and pitch, clamps the vertical angle,
    /// and positions the camera behind the player relative to the pivot and offset
    /// </summary>
    private void LateUpdate()
    {
        if (target == null || pivot == null)
            return;

        yaw += lookInput.x * currentSensitivity * Time.deltaTime;
        pitch += lookInput.y * currentSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        target.rotation = Quaternion.Euler(0f, yaw, 0f);
        pivot.localRotation = Quaternion.Euler(-pitch, 0f, 0f);

        transform.position = pivot.position + pivot.rotation * offset;
        transform.LookAt(pivot.position);
    }
}
