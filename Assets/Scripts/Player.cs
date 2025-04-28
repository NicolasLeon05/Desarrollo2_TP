using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    private Rigidbody _rigidBody;

    ForceRequest constantForceRequest;
    ForceRequest instantForceRequest;
    ForceRequest jumpRequest;

    [SerializeField] private int maxJumps = 2;
    public int jumps;

    private float groundIgnoreTime = 0.1f;
    private float lastJumpTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.5f;

    private float coyoteTime = 0.15f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void RequestInstantForce(ForceRequest forceRequest)
    {
        instantForceRequest = forceRequest;
    }

    public void RequestJump(ForceRequest forceRequest)
    {
        jumpRequest = forceRequest;
    }

    public void RequestConstantForce(ForceRequest forceRequest)
    {
        constantForceRequest = forceRequest;
    }

    private void FixedUpdate()
    {
        if (constantForceRequest != null)
            _rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.force, ForceMode.Force);

        if (jumpRequest != null)
        {
            lastJumpTime = Time.time;

            if (CheckGrounded() || Time.time - lastJumpTime <= coyoteTime && jumps < maxJumps)
            {
                jumps++;
                ResetJumpVelocity();

                _rigidBody.AddForce(jumpRequest.direction * jumpRequest.force, ForceMode.Impulse);
                jumpRequest = null;
                Debug.Log("JUMP");
            }
        }

        Debug.Log(jumps);
    }

    private void ResetJumpVelocity()
    {
        Vector3 velocity = _rigidBody.linearVelocity;
        velocity.y = 0;
        _rigidBody.linearVelocity = velocity;
    }

    private bool CheckGrounded()
    {
        if (Time.time - lastJumpTime < groundIgnoreTime)
            return true;

        Debug.Log("paso 1");

        jumpRayOrigin = transform.position + Vector3.down * 0.5f;
        JumpRayDirection = Vector3.down;

        if (Physics.Raycast(jumpRayOrigin, JumpRayDirection, jumpRayDistance))
        {
            jumps = 0;
            Debug.DrawRay(jumpRayOrigin, JumpRayDirection * jumpRayDistance, Color.red, 0.1f);
            return true;
        }
        else if (jumps == 0)
        {
            jumps = maxJumps;
        }

        return false;
    }
}
