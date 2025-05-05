using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;

    [SerializeField] private float speed;
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;

    Vector3 playerDirection = new Vector3(0, 0, 0);

    private InputBuffer jumpBuffer;
    [SerializeField] private float jumpBufferTime = 0.2f;


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

        if (dashAction != null)
        {
            dashAction.action.performed += OnDash;
            dashAction.action.canceled += OnDash;
        }
    }

    //MOVEMENT
    private void OnMove(InputAction.CallbackContext obj)
    {
        var request = new ForceRequest();
        var horizontalInput = obj.ReadValue<Vector2>();

        playerDirection = new Vector3(horizontalInput.x, 0, horizontalInput.y);

        request.direction = playerDirection;
        request.speed = speed;
        request.force = force;

        player.RequestConstantForce(request);
    }


    private void OnDash(InputAction.CallbackContext obj)
    {
        var request = new ForceRequest();

        request.direction = new Vector3(playerDirection.x, 0, playerDirection.z);
        request.speed = speed;
        request.force = force;

        player.RequestDash(request);
    }

    //JUMP
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
        return jumpForce;
    }
}
