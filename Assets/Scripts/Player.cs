using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    ForceRequest constantForceRequest;
    ForceRequest dashRequest;

    private Rigidbody rigidBody;

    //Jump
    [SerializeField] private int maxJumps = 2;
    private int jumps;
    private bool isOnAir;

    //Ground detection
    private bool isOnCoyoteTime;
    private bool isOnGround;
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
        rigidBody = GetComponent<Rigidbody>();
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
        if (IsDashing())
            return;
        else if (dashActivated)
        {
            SetPreDashVelocity();
            dashActivated = false;
            rigidBody.useGravity = true;
        }

        CheckGrounded();

        if (constantForceRequest != null)
        {
            if (!IsOverVelocityLimit())
                rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed, ForceMode.Force);

            constantForceRequest = null;
        }
        else
        {
            //Debug.Log("NOT MOVING");
            if (isOnGround && !isOnAir)
            {
                rigidBody.linearVelocity = rigidBody.linearVelocity * (maxSpeed * 0.1f);
                Debug.Log("aahhh");
            }
        }

        //Dash
        if (isOnCoyoteTime && !canDash)
        {
            canDash = true;
            dashRequest = null;
        }
        if (dashRequest != null && canDash)
            Dash();


        //Jump
        if (controller.HasBufferedJump())
        {
            if (isOnCoyoteTime)
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
        previousVelocity = rigidBody.linearVelocity;
        Vector3 dashVelocity = dashRequest.direction.normalized * dashRequest.force;
        rigidBody.linearVelocity = new Vector3(dashVelocity.x, 0, dashVelocity.z);

        rigidBody.useGravity = false;

        dashActivated = true;
        canDash = false;
        dashStartTime = Time.time;

        dashRequest = null;
    }

    private void SetPreDashVelocity()
    {
        Vector3 newVelocity = new Vector3(previousVelocity.x, 0, previousVelocity.z);
        rigidBody.linearVelocity = newVelocity;
    }

    private bool IsDashing()
    {
        return Time.time - dashStartTime < dashDuration;
    }

    private bool IsOverVelocityLimit()
    {
        Vector3 horizontal = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);
        return horizontal.magnitude > maxSpeed;
    }

    private void Jump()
    {
        isOnAir = true;
        jumps++;
        ResetJumpVelocity();

        float jumpForce = controller.GetJumpForce();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        controller.ConsumeBufferedJump();

        lastJumpTime = Time.time;
    }

    private void ResetJumpVelocity()
    {
        Vector3 velocity = rigidBody.linearVelocity;
        velocity.y = 0;
        rigidBody.linearVelocity = velocity;
    }

    private void CheckGrounded()
    {
        if (Time.time - lastJumpTime < groundIgnoreTime)
            return;

        jumpRayOrigin = transform.position + Vector3.down * 0.5f;
        JumpRayDirection = Vector3.down;

        if (Physics.Raycast(jumpRayOrigin, JumpRayDirection, jumpRayDistance))
        {
            isOnGround = true;
            isOnAir = false;
            lastGroundedTime = Time.time;
            jumps = 0;
            //Debug.Log("Touching ground");
        }
        else
        {
            isOnGround = false;
            isOnAir = true;
        }

        isOnCoyoteTime = (Time.time - lastGroundedTime <= coyoteTime);
    }

}
