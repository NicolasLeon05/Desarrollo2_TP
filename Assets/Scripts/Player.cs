using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    ForceRequest constantForceRequest;
    ForceRequest instantForceRequest;
    ForceRequest jumpRequest;


    private float groundIgnoreTime = 0.1f;
    private float lastJumpTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.5f;

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
        CheckGrounded();

        if (constantForceRequest != null)
            _rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.force, ForceMode.Force);

        if (jumpRequest != null)
        {
            if (jumps < maxJumps)
            {
                jumps++;
                ResetJumpVelocity();

                _rigidBody.AddForce(jumpRequest.direction * jumpRequest.force, ForceMode.Impulse);
                jumpRequest = null;
                lastJumpTime = Time.time;
                Debug.Log("JUMP");
            }
        }  
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
            jumps = 0;
            Debug.DrawRay(jumpRayOrigin, JumpRayDirection * jumpRayDistance, Color.red, 0.1f);
        }
    }
}
