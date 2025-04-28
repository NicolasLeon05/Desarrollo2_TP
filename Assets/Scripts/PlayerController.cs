using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;

    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _jumpForce;

    [SerializeField] private float jumpBufferTime = 0.1f;

    private InputBuffer jumpBuffer;

    private void Awake()
    {
        jumpBuffer = new InputBuffer(jumpBufferTime);
    }

    private void OnEnable()
    {
        if (moveAction == null)
            return;

        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;

        jumpAction.action.performed += OnJump;
        dashAction.action.performed += OnDash;
    }

    private void FixedUpdate()
    {
        if (jumpBuffer.Consume())
        {
            var request = new ForceRequest();
            request.direction = Vector3.up;
            request.speed = _speed;
            request.force = _jumpForce;

            player.RequestJump(request);
        }
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        ForceRequest request = RequestHorizontalForce(obj);

        player.RequestConstantForce(request);
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        ForceRequest request = RequestHorizontalForce(obj);

        player.RequestInstantForce(request);
    }

    private ForceRequest RequestHorizontalForce(InputAction.CallbackContext obj)
    {
        var request = new ForceRequest();
        var horizontalInput = obj.ReadValue<Vector2>();

        request.direction = new Vector3(horizontalInput.x, 0, horizontalInput.y);
        request.speed = _speed;
        request.force = _force;
        return request;
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        jumpBuffer.Register();
    }
}