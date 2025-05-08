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
    Vector2 rawMoveInput;

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

    private void Update()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        //There is an input
        if (rawMoveInput.magnitude > 0.01f)
        {
            Vector3 inputDir = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);

            Transform camTransform = Camera.main.transform;
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

            playerDirection = moveDir.normalized;

            var request = new ForceRequest
            {
                direction = playerDirection,
                speed = speed,
                force = force
            };

            player.RequestConstantForce(request);
        }
        //else
        //{
        //    var request = new ForceRequest
        //    {
        //        direction = Vector3.zero,
        //        speed = 0,
        //        force = 0
        //    };
        //
        //    player.RequestInstantForce(request);
        //}
    }

    //MOVEMENT WITH ROTATION
    private void OnMove(InputAction.CallbackContext obj)
    {
        rawMoveInput = obj.ReadValue<Vector2>();
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
