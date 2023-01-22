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
    
    [Space] [Header("Objects")] [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheckPositionL;
    [SerializeField] private Transform groundCheckPositionM;
    [SerializeField] private Transform groundCheckPositionR;
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private Transform ceilingCheckPosition;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnimator;
    
    [Space] [Header("Velocity")] [SerializeField] private Vector2 velocity;
    [SerializeField] private float horizontalVelocityImpulse;
    [SerializeField] private Vector2 moveInput;
    
    [Space] [Header("Movement Speed : Lerp is acceleration")] [SerializeField] private float moveSpeed;
    [Tooltip("Fields Labeled as 'Lerp', are used to control how quickly speed ramps up and down," +
             " i.e acceleration. ")] 
    [SerializeField] private float moveLerp;
    [SerializeField] private float impulseLerp;
    
    [Space] [Header("Jumping")] [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckMaxDistance;
    private bool _jumpReleaseAvailable;
    private bool _secondJumpAvailable;
    private bool _secondJumpConsumed;

    [Space] [Header("Wall Jumping")] [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallMoveSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private float wallGravity;
    [SerializeField] private float maxWallDistance;
    [SerializeField] private bool wallCheckDisableInvoked;

    private float _wallDirection;
    private bool _canWallJump;
    private bool _wallJumpEnabled;
    private bool _hasWallJumped;
    private float _wallTime;

    private void Start()
    {
        _wallJumpEnabled = true;
    }

    private void Update()
    {
        velocity.x = ((playerState == PlayerState.Clinging ? wallMoveSpeed : moveSpeed) * moveInput.x) * Time.deltaTime;

        if (_hasWallJumped && Math.Abs(moveInput.x - (-_wallDirection)) < 0.1)
        {
            velocity.x *= 0.2f;
        }

        horizontalVelocityImpulse = Mathf.Lerp(horizontalVelocityImpulse, 0, impulseLerp * Time.deltaTime);

        Vector2 tempVelocity = new Vector2(Mathf.Lerp(rb.velocity.x, (velocity.x * 1000f), moveLerp * Time.deltaTime), velocity.y);
       
        rb.velocity = tempVelocity + new Vector2(horizontalVelocityImpulse, 0f);

        RaycastHit groundHit;
        if (Physics.Raycast(groundCheckPositionL.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance)
            || Physics.Raycast(groundCheckPositionR.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance)
                || Physics.Raycast(groundCheckPositionM.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance))
        {
            if (velocity.y < 0)
            {
                playerState = PlayerState.Grounded;
                velocity.y = -0.01f;

                _jumpReleaseAvailable = false;

                _secondJumpAvailable = false;
                _secondJumpConsumed = false;
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

        // Gravity.
        if (playerState == PlayerState.Airborne)
            velocity.y += gravity * Time.deltaTime;

        if (playerState == PlayerState.Clinging)
            velocity.y += wallGravity * Time.deltaTime;

        // Animation.

        if (velocity.x > 0f)
        {
            playerSprite.flipX = false;
        }

        if (velocity.x < 0f)
        {
            playerSprite.flipX = true;
        }

        if (playerState == PlayerState.Clinging)
            _wallTime += Time.deltaTime;
        else
            _wallTime = 0.0f;
        
        playerAnimator.SetBool("Moving", moveInput.x != 0);
        playerAnimator.SetBool("Grounded", playerState == PlayerState.Grounded);
        playerAnimator.SetFloat("YVelocity", velocity.y);
        playerAnimator.SetFloat("WallTime", _wallTime);
    }

    private void CheckWall()
    {
        if (!_wallJumpEnabled)
            return;
        
        if (playerState == PlayerState.Grounded)
            return;

        RaycastHit wallHit;
        if (Physics.Raycast(wallCheckPosition.position, new Vector3(moveInput.x, 0, 0), out wallHit, maxWallDistance))
        {
            if (!_canWallJump)
            {
                _wallDirection = moveInput.x;
                wallJumpDirection = new Vector2(wallJumpDirection.x, wallJumpDirection.y);

                _canWallJump = true;

                if (velocity.y > 0.1f)
                    velocity.y = 0.0f;
            }
            
            playerState = PlayerState.Clinging;
        }
        else if (_canWallJump && !wallCheckDisableInvoked)
        {
            Invoke(nameof(DisableWallJump), 0.2f);
            wallCheckDisableInvoked = true;
            playerState = PlayerState.Airborne;
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (playerState == PlayerState.Grounded || (_secondJumpAvailable && playerState != PlayerState.Clinging))
        {
            if (!_secondJumpConsumed)
            {
                velocity.y += jumpForce;

                Invoke(nameof(EnableSecondJump), 0.3f);
            }
            else
            {
                velocity.y = jumpForce;
            }
            
            _secondJumpAvailable = false;
            
            Invoke(nameof(EnableJumpRelease), 0.1f);
        }

        if (_canWallJump)
        {
            velocity.y = wallJumpDirection.y * wallJumpForce;
            horizontalVelocityImpulse += (wallJumpDirection.x * -_wallDirection) * wallJumpForce;
            
            _wallJumpEnabled = false;
            _canWallJump = false;
            _hasWallJumped = true;
            
            playerState = PlayerState.Airborne;
            
            Invoke(nameof(EnableWallJump), 0.25f);
            Invoke(nameof(EndWallJump), 0.25f);
            Invoke(nameof(EnableSecondJump), 0.3f);
        }
    }

    private void OnJumpRelease(InputValue value)
    {
        if (_secondJumpConsumed)
            return;;
        
        if (value.Get<float>() == 0 && _jumpReleaseAvailable && velocity.y > 0.1f)
        {
            velocity.y = 0.1f;

            _jumpReleaseAvailable = false;
        }
    }

    private void EndWallJump()
    {
        _hasWallJumped = false;
    }

    private void EnableJumpRelease()
    {
        _jumpReleaseAvailable = true;
    }

    private void DisableWallJump()
    {
        _canWallJump = false;
        wallCheckDisableInvoked = false;
    }

    private void EnableWallJump()
    {
        _wallJumpEnabled = true;
    }

    private void EnableSecondJump()
    {
        _secondJumpAvailable = true;
        _secondJumpConsumed = true;
    }
}
