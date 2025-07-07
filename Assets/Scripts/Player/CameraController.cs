using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // El transform del jugador
    [SerializeField] private Transform pivot;  // Hijo del jugador, usado para rotación vertical
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

        TryAssignPlayerAndPivot();
    }

    private void TryAssignPlayerAndPivot()
    {
        if (target == null && Player.Instance != null)
        {
            target = Player.Instance.transform;
        }

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target != null && pivot == null)
        {
            Transform foundPivot = target.Find("Pivot");
            if (foundPivot != null)
            {
                pivot = foundPivot;
            }
       
            else
            {
                Debug.LogWarning("Pivot not found");
            }
        }

        if (target == null)
            Debug.LogWarning("Target for camera not found");
    }

    private void OnMoveCamera(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        if (target == null || pivot == null)
            return;

        yaw += lookInput.x * sensitivity;
        pitch += lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        target.rotation = Quaternion.Euler(0f, yaw, 0f);
        pivot.localRotation = Quaternion.Euler(-pitch, 0f, 0f);

        transform.position = pivot.position + pivot.rotation * offset;
        transform.LookAt(pivot.position);
    }
}
