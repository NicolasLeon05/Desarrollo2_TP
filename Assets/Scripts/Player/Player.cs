using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private IJumpProvider jumpProvider;

    [SerializeField] private Transform spawnPoint;

    private ForceRequest constantForceRequest;
    private ForceRequest dashRequest;
    private ForceRequest flyRequest;

    private Rigidbody rigidBody;

    [Header("Jump")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float airVelocityMultiplier;
    private int jumps;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnAngleLimitDown;
    [SerializeField] private float turnAngleLimitUp;

    [Header("Ground detection")]
    private bool isOnCoyoteTime;
    private bool isOnGround;
    private float groundIgnoreTime = 0.3f;
    private float lastJumpTime;
    private float lastGroundedTime;
    private Vector3 jumpRayOrigin;
    private Vector3 JumpRayDirection;
    [SerializeField] private float jumpRayDistance = 0.15f;
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashDuration = 0.3f;
    private Vector3 previousVelocity;
    private float dashStartTime = 0f;
    private bool dashActivated = false;
    private bool canDash = true;

    [Header("Brake")]
    private float brakeForce = 10f;


    [Header("Cheats")]
    public bool hasFlyCheat = false;
    public bool hasSpeedCheat = false;
    [SerializeField] private float maxSpeedWithCheat = 50f;


    private Animator animator;

    public static Player Instance { get; private set; }

    /// <summary>
    /// Initializes the player instance, Rigidbody, Animator, and optional spawn position.
    /// Ensures only one instance exists using Singleton pattern.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Instance.ApplyTeleportCheat(spawnPoint);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        jumpProvider = GetComponent<IJumpProvider>();

        if (spawnPoint)
            rigidBody.position = spawnPoint.position;
    }

    /// <summary>
    /// Stores a dash force request
    /// </summary>
    public void RequestDash(ForceRequest forceRequest)
    {
        dashRequest = forceRequest;
    }

    /// <summary>
    /// Stores a constant force request
    /// </summary>
    public void RequestConstantForce(ForceRequest forceRequest)
    {
        constantForceRequest = forceRequest;
    }

    /// <summary>
    /// Stores a vertical fly force request
    /// </summary>
    public void RequestFlyForce(ForceRequest forceRequest)
    {
        flyRequest = forceRequest;
    }

    /// <summary>
    /// Handles animation updates, dash, ground detection, movement, jump and fly cheat.
    /// </summary>
    private void FixedUpdate()
    {
        UpdateAnimationStates();

        if (IsDashing())
            return;

        if (dashActivated)
        {
            RestorePreDashVelocity();
            dashActivated = false;
            rigidBody.useGravity = true;
        }

        CheckGrounded();

        ManageHorizontalMovement();

        if ((isOnCoyoteTime && !canDash))
        {
            canDash = true;
            dashRequest = null;
        }

        if (dashRequest != null && canDash)
            Dash();

        ManageJump();


        if (hasFlyCheat)
            ManageFly();

    }

    /// <summary>
    /// Manages jumping based on coyote time and remaining jumps
    /// </summary>
    private void ManageJump()
    {
        if (jumpProvider.HasBufferedJump())
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

    /// <summary>
    /// Handles horizontal movement or applies braking when idle.
    /// </summary>
    private void ManageHorizontalMovement()
    {
        if (constantForceRequest != null)
        {
            MoveHorizontally();

            constantForceRequest = null;
        }
        else
        {
            ApplyBrake();
        }
    }

    /// <summary>
    /// Applies horizontal force based on input and limits horizontal velocity
    /// </summary>
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
            float yVelocity = rigidBody.linearVelocity.y;

            Vector3 velocity = rigidBody.linearVelocity;
            velocity.y = 0;
            velocity.Normalize();
            velocity *= maxSpeed;
            velocity.y = yVelocity;
            rigidBody.linearVelocity = velocity;
        }
        animator.SetFloat("Speed",rigidBody.linearVelocity.magnitude);
    }

    /// <summary>
    /// Adjusts player's linear velocity if movement direction changed too much
    /// </summary>
    private void ManageMovementAngleChange()
    {
        Vector3 newDirection = constantForceRequest.direction;
        newDirection.y = 0;
        newDirection.Normalize();

        Vector3 currentDirection = rigidBody.linearVelocity;
        currentDirection.y = 0;
        currentDirection.Normalize();
        float angleChange = Vector3.Angle(newDirection, currentDirection);


        if (angleChange > turnAngleLimitDown && angleChange < turnAngleLimitUp)
            AdjustVelocityToAngle(newDirection);
    }

    /// <summary>
    /// Rotates current velocity toward the new movement direction.
    /// </summary>
    private void AdjustVelocityToAngle(Vector3 newDirection)
    {
        newDirection *= rigidBody.linearVelocity.magnitude;
        rigidBody.linearVelocity = newDirection;
    }

    /// <summary>
    /// Applies braking force to slow down the player
    /// </summary>
    private void ApplyBrake()
    {
        if (isOnGround)
            rigidBody.AddForce(rigidBody.linearVelocity * -1 * brakeForce, ForceMode.Force);
        else if (hasFlyCheat)
        {
            Vector3 velocity = rigidBody.linearVelocity;
            velocity.x *= 0.1f;
            velocity.z *= 0.1f;
            rigidBody.linearVelocity = velocity;
        }

    }

    /// <summary>
    /// Handles vertical movement using fly force when the cheat is active
    /// </summary>
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

    /// <summary>
    /// Executes a dash movement, overrides velocity, disables gravity temporarily.
    /// </summary>
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

    /// <summary>
    /// Restores previous velocity after a dash finishes
    /// </summary>
    private void RestorePreDashVelocity()
    {
        if (previousVelocity.magnitude < 0.1f)
            return;

        Vector3 direction = transform.forward;
        Vector3 newVelocity = direction * previousVelocity.magnitude;

        rigidBody.linearVelocity = newVelocity;
    }

    /// <summary>
    /// Returns whether the player is currently in the middle of a dash
    /// </summary>
    public bool IsDashing()
    {
        return Time.time - dashStartTime < dashDuration;
    }

    /// <summary>
    /// Returns whether the player's horizontal velocity os higher than the max speed allowed.
    /// Takes cheat mode in consideration
    /// </summary>
    private bool IsOverVelocityLimit()
    {
        Vector3 horizontal = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);

        if (!hasSpeedCheat)
            return horizontal.magnitude > maxSpeed;
        else
            return horizontal.magnitude > maxSpeedWithCheat;
    }

    /// <summary>
    /// Executes a jump by applying upward impulse force
    /// </summary>
    private void Jump()
    {
        float jumpForce = jumpProvider.GetJumpForce();
        jumps++;
        ResetVelocityY();

        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jumpProvider.ConsumeBufferedJump();
        lastJumpTime = Time.time;

    }

    /// <summary>
    /// Sets vertical velocity to zero
    /// </summary>
    private void ResetVelocityY()
    {
        Vector3 velocity = rigidBody.linearVelocity;
        velocity.y = 0;
        rigidBody.linearVelocity = velocity;
    }

    /// <summary>
    /// Uses a raycast to check whether the player is grounded.
    /// Calculates coyote time and resets jump count
    /// </summary>
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

    /// <summary>
    /// Updates the player's animation parameters based on movement state
    /// </summary>
    private void UpdateAnimationStates()
    {
        bool isJumping = rigidBody.linearVelocity.y > 0.1f && !isOnGround;
        bool isFalling = rigidBody.linearVelocity.y < -0.1f && !isOnGround;

        Vector3 aux = rigidBody.linearVelocity;
        aux.y = 0;

        animator.SetFloat("Speed", aux.magnitude);
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isDashing", dashActivated);
    }

    /// <summary>
    /// Toggles the fly cheat on or off, affecting gravity
    /// </summary>
    public void ApplyFlyCheat()
    {
        hasFlyCheat = !hasFlyCheat;
        rigidBody.useGravity = !hasFlyCheat;
        Debug.Log("Toggle fly cheat = " + hasFlyCheat);
    }

    /// <summary>
    /// Toggles the speed cheat on or off
    /// </summary>
    public void ApplySpeedCheat()
    {
        hasSpeedCheat = !hasSpeedCheat;
        Debug.Log("Toggle speed cheat = " + hasSpeedCheat);
    }

    /// <summary>
    /// Instantly moves the player to the specified goal
    /// </summary>
    public void ApplyTeleportCheat(Transform goal)
    {
        transform.position = goal.position;
    }

}
