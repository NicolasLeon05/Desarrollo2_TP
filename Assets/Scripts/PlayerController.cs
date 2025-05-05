using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _jumpForce;

    [SerializeField] private float jumpBufferTime = 0.2f;

    private InputBuffer jumpBuffer;

    private void Awake()
    {
        jumpBuffer = new InputBuffer(jumpBufferTime);
    }

    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.performed += OnMove;
            moveAction.action.canceled += OnMove;
        }

        if (jumpAction != null)
        {
            jumpAction.action.performed += OnJump;
        }
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var request = new ForceRequest();
        var horizontalInput = obj.ReadValue<Vector2>();

        request.direction = new Vector3(horizontalInput.x, 0, horizontalInput.y);
        request.speed = _speed;
        request.force = _force;

        player.RequestConstantForce(request);
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        jumpBuffer.Register();
    }

    public bool HasBufferedJump()
    {
        return jumpBuffer.Peek();
    }

    public void ConsumeBufferedJump()
    {
        jumpBuffer.Consume();
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }
}
