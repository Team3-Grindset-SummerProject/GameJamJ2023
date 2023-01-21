using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private enum PlayerState
    {
        Grounded,
        Airborne,
        Clinging
    }

    [SerializeField] private PlayerState playerState;
    
    [Space] [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private Transform ceilingCheckPosition;

    [Space] [SerializeField] private Vector2 velocity;
    [SerializeField] private float horizontalVelocityImpulse;
    [SerializeField] private Vector2 moveInput;
    
    [Space] [SerializeField] private float moveSpeed;
    [SerializeField] private float moveLerp;
    [SerializeField] private float impulseLerp;
    
    [Space] [SerializeField] private float jumpForce;
    [SerializeField] private bool jumpReleaseAvailable;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckMaxDistance;
    [SerializeField] private bool secondJumpAvailable;
    [SerializeField] private bool secondJumpConsumed;

    [Space] [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallMoveSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private float wallGravity;
    [SerializeField] private float wallDirection;
    [SerializeField] private float maxWallDistance;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool wallCheckDisableInvoked;
    [SerializeField] private bool wallJumpEnabled;
    [SerializeField] private bool hasWallJumped;

    private void Update()
    {
        velocity.x = ((playerState == PlayerState.Clinging ? wallMoveSpeed : moveSpeed) * moveInput.x) * Time.deltaTime;

        if (hasWallJumped && Math.Abs(moveInput.x - (-wallDirection)) < 0.1)
        {
            velocity.x *= 0.2f;
        }

        horizontalVelocityImpulse = Mathf.Lerp(horizontalVelocityImpulse, 0, impulseLerp * Time.deltaTime);

        Vector2 tempVelocity = new Vector2(Mathf.Lerp(rb.velocity.x, (velocity.x * 1000f), moveLerp * Time.deltaTime), velocity.y);
       
        rb.velocity = tempVelocity + new Vector2(horizontalVelocityImpulse, 0f);

        RaycastHit groundHit;
        if (Physics.Raycast(groundCheckPosition.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance))
        {
            if (velocity.y < 0)
            {
                playerState = PlayerState.Grounded;
                velocity.y = -0.01f;

                jumpReleaseAvailable = false;

                secondJumpAvailable = false;
                secondJumpConsumed = false;
            }
        }
        else if (playerState != PlayerState.Clinging)
        {
            playerState = PlayerState.Airborne;
        }
        
        RaycastHit ceilingHit;
        if (Physics.Raycast(ceilingCheckPosition.position, transform.TransformDirection(Vector3.up), out ceilingHit, groundCheckMaxDistance))
        {
            if (velocity.y > 0.1f)
            {
                velocity.y = 0;
            }
        }
        
        CheckWall();

        if (canWallJump)
            playerState = PlayerState.Clinging;

        // Gravity.
        if (playerState == PlayerState.Airborne)
            velocity.y += gravity * Time.deltaTime;

        if (playerState == PlayerState.Clinging)
            velocity.y += wallGravity * Time.deltaTime;
    }

    private void CheckWall()
    {
        if (!wallJumpEnabled)
            return;
        
        if (playerState == PlayerState.Grounded)
            return;

        RaycastHit wallHit;
        if (Physics.Raycast(wallCheckPosition.position, new Vector3(moveInput.x, 0, 0), out wallHit, maxWallDistance))
        {
            if (!canWallJump)
            {
                wallDirection = moveInput.x;
                wallJumpDirection = new Vector2(wallJumpDirection.x, wallJumpDirection.y);

                canWallJump = true;

                if (velocity.y > 0.1f)
                    velocity.y = 0.0f;
            }
        }
        else if (canWallJump && !wallCheckDisableInvoked)
        {
            Invoke(nameof(DisableWallJump), 0.2f);
            wallCheckDisableInvoked = true;
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (playerState == PlayerState.Grounded || (secondJumpAvailable && playerState != PlayerState.Clinging))
        {
            if (!secondJumpConsumed)
            {
                velocity.y += jumpForce;

                Invoke(nameof(EnableSecondJump), 0.3f);
            }
            else
            {
                velocity.y = jumpForce;
            }
            
            secondJumpAvailable = false;
            
            Invoke(nameof(EnableJumpRelease), 0.1f);
        }

        if (playerState == PlayerState.Clinging)
        {
            velocity.y = wallJumpDirection.y * wallJumpForce;
            horizontalVelocityImpulse += (wallJumpDirection.x * -wallDirection) * wallJumpForce;
            
            wallJumpEnabled = false;
            canWallJump = false;
            hasWallJumped = true;
            
            playerState = PlayerState.Airborne;
            
            Invoke(nameof(EnableWallJump), 0.25f);
            Invoke(nameof(EndWallJump), 0.25f);
            Invoke(nameof(EnableSecondJump), 0.3f);
        }
    }

    private void OnJumpRelease(InputValue value)
    {
        if (secondJumpConsumed)
            return;;
        
        if (value.Get<float>() == 0 && jumpReleaseAvailable && velocity.y > 0.1f)
        {
            velocity.y = 0.1f;

            jumpReleaseAvailable = false;
        }
    }

    private void EndWallJump()
    {
        hasWallJumped = false;
    }

    private void EnableJumpRelease()
    {
        jumpReleaseAvailable = true;
    }

    private void DisableWallJump()
    {
        canWallJump = false;
        wallCheckDisableInvoked = false;
    }

    private void EnableWallJump()
    {
        wallJumpEnabled = true;
    }

    private void EnableSecondJump()
    {
        secondJumpAvailable = true;
        secondJumpConsumed = true;
    }
}
