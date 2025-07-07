using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference flyUpAction;
    [SerializeField] private InputActionReference flyDownAction;

    [SerializeField] private float speed;
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float flySpeed;
    [SerializeField] private float speedCheatMultiplier = 5f;

    Vector3 playerDirection = Vector3.zero;
    Vector2 rawMoveInput;

    float flyUpInput = 0;
    float flyDownInput = 0;

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

        if (flyUpAction != null)
        {
            flyUpAction.action.performed += OnFlyUp;
            flyUpAction.action.canceled += OnFlyUp;
        }

        if (flyDownAction != null)
        {
            flyDownAction.action.performed += OnFlyDown;
            flyDownAction.action.canceled += OnFlyDown;
        }
    }


    private void Update()
    {
        CheckMovement();
        CheckFly();
    }

    private void CheckMovement()
    {
        if (rawMoveInput.magnitude > 0.01f)
        {
            RotatePlayerToCamera();

            var request = new ForceRequest
            {
                direction = playerDirection,
                speed = speed,
                force = force
            };

            if (player.hasSpeedCheat)
            {
                request.speed *= speedCheatMultiplier;
                request.force *= speedCheatMultiplier;
            }

            player.RequestConstantForce(request);
        }
    }

    private void CheckFly()
    {
        if (!player.hasFlyCheat)
            return;

        if (flyUpInput > 0.01f)
        {
            var request = new ForceRequest
            {
                direction = Vector3.up,
                speed = flySpeed,
                force = force
            };

            player.RequestFlyForce(request);
        }

        if (flyDownInput > 0.01f)
        {
            var request = new ForceRequest
            {
                direction = Vector3.down,
                speed = flySpeed,
                force = force
            };

            player.RequestFlyForce(request);
        }
    }

    //MOVEMENT WITH ROTATION
    private void OnMove(InputAction.CallbackContext obj)
    {
        rawMoveInput = obj.ReadValue<Vector2>();
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        RotatePlayerToCamera();

        var request = new ForceRequest
        {
            direction = player.transform.forward,
            speed = speed,
            force = force
        };

        player.RequestDash(request);
    }

    private void RotatePlayerToCamera()
    {
        Vector3 inputDir = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);

        Transform camTransform = Camera.main.transform;
        if (camTransform != null)
        {
            Debug.Log("Camera.main: " + camTransform.name + " (escena: " + camTransform.gameObject.scene.name + ")");
        }
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        playerDirection = moveDir.normalized;
    }

    //JUMP
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!player.hasFlyCheat)
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

    private void OnFlyUp(InputAction.CallbackContext context)
    {
        flyUpInput = context.ReadValue<float>();
    }

    private void OnFlyDown(InputAction.CallbackContext context)
    {
        flyDownInput = context.ReadValue<float>();
    }
}