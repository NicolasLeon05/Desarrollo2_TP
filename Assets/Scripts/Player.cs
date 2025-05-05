using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    ForceRequest constantForceRequest;
    ForceRequest instantForceRequest;
    //ForceRequest jumpRequest;

    [SerializeField] PlayerController controller;

    private float groundIgnoreTime = 0.1f;
    private float lastJumpTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.5f;

    private float lastGroundedTime;
    [SerializeField] private float coyoteTime = 0.2f; // 200ms de margen
    private bool isGrounded;

    private Rigidbody _rigidBody;

    [SerializeField] private int maxJumps = 2;
    public int jumps;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void RequestInstantForce(ForceRequest forceRequest)
    {
        instantForceRequest = forceRequest;
    }

    //public void RequestJump(ForceRequest forceRequest)
    //{
    //    jumpRequest = forceRequest;
    //}

    public void RequestConstantForce(ForceRequest forceRequest)
    {
        constantForceRequest = forceRequest;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (constantForceRequest != null)
            _rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.force, ForceMode.Force);

        if (controller.HasBufferedJump())
        {
            bool onGround = (Time.time - lastGroundedTime <= coyoteTime);

            if (onGround)
            {
                Jump();
            }
            else if (jumps < maxJumps)
            {
                jumps = maxJumps - 1;
                Jump();
            }
        }

        Debug.Log(_rigidBody.linearVelocity.y);
    }

    private void Jump()
    {
        jumps++;
        ResetJumpVelocity();

        float jumpForce = controller.GetJumpForce();
        _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        controller.ConsumeBufferedJump();

        lastJumpTime = Time.time;
    }

    private void ResetJumpVelocity()
    {
        Vector3 velocity = _rigidBody.linearVelocity;
        velocity.y = 0;
        _rigidBody.linearVelocity = velocity;
    }

    private void CheckGrounded()
    {
        if (Time.time - lastJumpTime < groundIgnoreTime)
            return;

        jumpRayOrigin = transform.position + Vector3.down * 0.5f;
        JumpRayDirection = Vector3.down;

        if (Physics.Raycast(jumpRayOrigin, JumpRayDirection, jumpRayDistance))
        {
            isGrounded = true;
            lastGroundedTime = Time.time;
            jumps = 0;
            Debug.DrawRay(jumpRayOrigin, JumpRayDirection * jumpRayDistance, Color.red, 0.1f);
        }
        else
        {
            isGrounded = false;
        }
    }

}
