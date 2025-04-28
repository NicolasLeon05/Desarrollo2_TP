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

    [SerializeField] private float lastGroundTouchedTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.5f;

    [SerializeField] private float coyoteTime = 0.15f;

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

        if (instantForceRequest != null)
            _rigidBody.AddForce(instantForceRequest.direction * instantForceRequest.force, ForceMode.Impulse);

        if (CheckGrounded())
            jumps = 0;

        if (jumpRequest != null)
        {
            if (jumps < maxJumps)
            {
                jumps++;
                ResetJumpVelocity();

                _rigidBody.AddForce(jumpRequest.direction * jumpRequest.force, ForceMode.Impulse);
                jumpRequest = null;
            }
        }

    }

    private bool CheckGrounded()
    {
        jumpRayOrigin = transform.position + Vector3.down * 0.5f;
        JumpRayDirection = Vector3.down;

        if (Physics.Raycast(jumpRayOrigin, JumpRayDirection, jumpRayDistance))
        {
            lastGroundTouchedTime = Time.time;
            return true;
        }
        else if (CheckCoyoteTime())
            return true;

        return false;
    }


    private bool CheckCoyoteTime()
    {
        return Time.time - lastGroundTouchedTime <= coyoteTime;
    }

    private void ResetJumpVelocity()
    {
        Vector3 velocity = _rigidBody.linearVelocity;
        velocity.y = 0;
        _rigidBody.linearVelocity = velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(jumpRayOrigin, jumpRayOrigin + JumpRayDirection * jumpRayDistance);
    }
}
