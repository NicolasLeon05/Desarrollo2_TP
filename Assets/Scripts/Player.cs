using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] Transform spawnPoint;

    private ForceRequest constantForceRequest;
    private ForceRequest dashRequest;

    private Rigidbody rigidBody;

    //Jump
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float airVelocityMultiplier;
    private int jumps;

    //Ground detection
    private bool isOnCoyoteTime;
    private bool isOnGround;
    private float groundIgnoreTime = 0.3f;
    private float lastJumpTime;
    private float lastGroundedTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.15f;
    [SerializeField] private float coyoteTime = 0.2f;

    //Dash
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float dashDuration = 0.3f;
    private Vector3 previousVelocity;
    private float dashStartTime = 0f;
    private bool dashActivated = false;
    private bool canDash = true;

    //Cheats
    bool hasFlyCheat = false;
    bool hasSpeedCheat = false;

    //Animations
    private Animator animator;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (spawnPoint)
            rigidBody.position = spawnPoint.position;
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

        if (dashActivated)
        {
            SetPreDashVelocity();
            dashActivated = false;
            rigidBody.useGravity = true;
        }

        CheckGrounded();

        if (constantForceRequest != null)
        {
            if (!IsOverVelocityLimit())
                if (isOnGround)
                    rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed, ForceMode.Force);
                else
                    rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed * airVelocityMultiplier, ForceMode.Force);

            constantForceRequest = null;
        }
        else
        {
            if (isOnGround)
                rigidBody.linearVelocity = rigidBody.linearVelocity * 0.1f;
        }

        if (isOnCoyoteTime && !canDash)
        {
            canDash = true;
            dashRequest = null;
        }

        if (dashRequest != null && canDash)
            Dash();

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

        UpdateAnimationStates();
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

        constantForceRequest = null;
        dashRequest = null;
    }

    private void SetPreDashVelocity()
    {
        if (previousVelocity.magnitude < 0.1f)
            return;

        Vector3 direction = transform.forward;
        Vector3 newVelocity = direction * previousVelocity.magnitude;

        rigidBody.linearVelocity = newVelocity;
    }

    public bool IsDashing()
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
        float jumpForce = controller.GetJumpForce();

        if (hasFlyCheat)
        {
            Fly(jumpForce);
        }
        else
        {
            jumps++;
            ResetJumpVelocity();

            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            controller.ConsumeBufferedJump();
            lastJumpTime = Time.time;
        }

    }

    private void Fly(float speed)
    {
        rigidBody.AddForce(Vector3.up * speed, ForceMode.Force);
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
        {
            isOnGround = false;
        }
        else
        {
            jumpRayOrigin = transform.position + Vector3.up * 0.1f;
            JumpRayDirection = Vector3.down;

            Debug.DrawRay(jumpRayOrigin, JumpRayDirection, Color.red);
            if (Physics.Raycast(jumpRayOrigin, JumpRayDirection, jumpRayDistance))
            {
                isOnGround = true;
                lastGroundedTime = Time.time;
                jumps = 0;
            }
            else
            {
                isOnGround = false;
            }

            isOnCoyoteTime = (Time.time - lastGroundedTime <= coyoteTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            transform.SetParent(null);
        }
    }

    private void UpdateAnimationStates()
    {
        bool isMoving = constantForceRequest != null;
        bool isJumping = rigidBody.linearVelocity.y > 0.1f && !isOnGround;
        bool isFalling = rigidBody.linearVelocity.y < -0.1f && !isOnGround;

        animator.SetBool("isRunning", isMoving);
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isDashing", IsDashing());
    }

    public void ApplyFlyCheat()
    {
        hasFlyCheat = !hasFlyCheat;
        rigidBody.useGravity = !hasFlyCheat;

        Debug.Log("Fly cheat = " + hasFlyCheat);
    }

    public void ApplySpeedCheat()
    {
        hasSpeedCheat = !hasSpeedCheat;
    }

}
