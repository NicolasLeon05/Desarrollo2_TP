using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    ForceRequest constantForceRequest;
    ForceRequest dashRequest;

    private Rigidbody _rigidBody;

    [SerializeField] private int maxJumps = 2;
    private int jumps;
    private bool isOnGround;

    //Ground detection
    private float groundIgnoreTime = 0.1f;
    private float lastJumpTime;
    private float lastGroundedTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.5f;
    [SerializeField] private float coyoteTime = 0.2f;

    //Dash
    Vector3 previousVelocity;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float dashDuration = 0.3f;
    private float dashStartTime = 0f;
    private bool dashActivated = false;
    private bool canDash = true;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void RequestDash(ForceRequest forceRequest)
    {
        dashRequest = forceRequest;
    }

    public void RequestConstantForce(ForceRequest forceRequest)
    {
        constantForceRequest = forceRequest;
    }


    private void FixedUpdate()
    {
        Debug.Log(_rigidBody.linearVelocity);

        if (IsDashing())
            return;
        else if (dashActivated)
        {
            ResetVelocity();
            dashActivated = false;
            _rigidBody.useGravity = true;
        }

        CheckGrounded();

        if (constantForceRequest != null)
            if (!IsOverVelocityLimit())
                _rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed, ForceMode.Force);

        //Dash
        if (isOnGround && !canDash)
        {
            canDash = true;
            dashRequest = null;
        }
        if (dashRequest != null && canDash)
            Dash();


        //Jump
        if (controller.HasBufferedJump())
        {       
            if (isOnGround)
            {
                Jump();
            }
            else if (jumps < maxJumps)
            {
                jumps = maxJumps - 1;
                Jump();
            }
        }
    }


    private void Dash()
    {
        previousVelocity = _rigidBody.linearVelocity;
        Vector3 dashVelocity = dashRequest.direction.normalized * dashRequest.force;
        _rigidBody.linearVelocity = new Vector3(dashVelocity.x, 0, dashVelocity.z);

        _rigidBody.useGravity = false;

        dashActivated = true;
        canDash = false;
        dashStartTime = Time.time;
        
        dashRequest = null;
    }

    private void ResetVelocity()
    {
        Vector3 newVelocity = new Vector3(previousVelocity.x, 0, previousVelocity.z);
        _rigidBody.linearVelocity = newVelocity;
    }

    private bool IsDashing()
    {
        return Time.time - dashStartTime < dashDuration;
    }

    private bool IsOverVelocityLimit()
    {
        Vector3 horizontal = new Vector3(_rigidBody.linearVelocity.x, 0, _rigidBody.linearVelocity.z);
        return horizontal.magnitude > maxSpeed;
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
            lastGroundedTime = Time.time;
            jumps = 0;
            Debug.DrawRay(jumpRayOrigin, JumpRayDirection * jumpRayDistance, Color.red, 0.1f);
        }

        isOnGround = (Time.time - lastGroundedTime <= coyoteTime);
    }

}
