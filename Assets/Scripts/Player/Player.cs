using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] Transform spawnPoint;

    private ForceRequest constantForceRequest;
    private ForceRequest dashRequest;
    private ForceRequest flyRequest;

    private Rigidbody rigidBody;

    //Jump
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float airVelocityMultiplier;
    private int jumps;

    //Movement
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnAngleLimitDown;
    [SerializeField] private float turnAngleLimitUp;

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
    [SerializeField] private float dashDuration = 0.3f;
    private Vector3 previousVelocity;
    private float dashStartTime = 0f;
    private bool dashActivated = false;
    private bool canDash = true;

    //Cheats
    public bool hasFlyCheat = false;
    public bool hasSpeedCheat = false;
    [SerializeField] private float maxSpeedWithCheat = 50f;

    //Animations
    private Animator animator;


    private static Player instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            instance.ApplyTeleportCheat(spawnPoint);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

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

    public void RequestFlyForce(ForceRequest forceRequest)
    {
        flyRequest = forceRequest;
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

        UpdateAnimationStates();

        CheckGrounded();

        //Horizontal movement
        ManageHorizontalMovement();

        //Dash
        if ((isOnCoyoteTime && !canDash))
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

        //Fly
        if (hasFlyCheat)
            ManageFly();

    }

    private void ManageHorizontalMovement()
    {
        if (constantForceRequest != null)
        {
            MoveHorizontally();

            constantForceRequest = null;
        }
        else
        {
            StopMovement();
        }
    }

    private void MoveHorizontally()
    {
        if (isOnGround)
        {
            rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed, ForceMode.Force);
            ManageMovementAngleChange();
        }
        else
            rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.speed * airVelocityMultiplier, ForceMode.Force);

        if (IsOverVelocityLimit())
        {
            Vector3 velocity = rigidBody.linearVelocity;
            velocity.y = 0;
            velocity.Normalize();
            velocity *= maxSpeed;
            rigidBody.linearVelocity = velocity;
        }
    }

    private void ManageMovementAngleChange()
    {
        Vector3 newDirection = constantForceRequest.direction.normalized;
        Vector3 currentDirection = rigidBody.linearVelocity.normalized;

        float angleChange = Vector3.Angle(newDirection, currentDirection);

        //Debug.Log("Angle change: " + angleChange);

        if (angleChange > turnAngleLimitDown && angleChange < turnAngleLimitUp)
            AdjustVelocityToAngle(newDirection);
    }

    private void AdjustVelocityToAngle(Vector3 newDirection)
    {
        newDirection *= rigidBody.linearVelocity.magnitude;
        rigidBody.linearVelocity = newDirection;
    }

    private void StopMovement()
    {
        if (isOnGround)
            rigidBody.linearVelocity *= 0.1f;
        else if (hasFlyCheat)
        {
            Vector3 velocity = rigidBody.linearVelocity;
            velocity.x *= 0.1f;
            velocity.z *= 0.1f;
            rigidBody.linearVelocity = velocity;
        }
    }

    private void ManageFly()
    {
        if (flyRequest != null)
        {
            rigidBody.AddForce(flyRequest.direction * flyRequest.speed, ForceMode.Force);
            flyRequest = null;
        }
        else
        {
            ResetVelocityY();
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

        if (!hasSpeedCheat)
            return horizontal.magnitude > maxSpeed;
        else
            return horizontal.magnitude > maxSpeedWithCheat;
    }

    private void Jump()
    {
        float jumpForce = controller.GetJumpForce();
        jumps++;
        ResetVelocityY();

        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        controller.ConsumeBufferedJump();
        lastJumpTime = Time.time;

    }

    private void ResetVelocityY()
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
                Debug.Log("Raycast Hit");
                isOnGround = true;
                lastGroundedTime = Time.time;
                jumps = 0;
            }
            else
            {
                isOnGround = false;
            }
        }

        isOnCoyoteTime = (Time.time - lastGroundedTime <= coyoteTime);
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
    }

    public void ApplySpeedCheat()
    {
        hasSpeedCheat = !hasSpeedCheat;
        Debug.Log("Toggle speed cheat = " + hasSpeedCheat);
    }

    public void ApplyTeleportCheat(Transform goal)
    {
        transform.position = goal.position;
    }

}
