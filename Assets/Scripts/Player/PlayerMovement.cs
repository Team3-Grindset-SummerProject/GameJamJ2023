using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [SerializeField] private GameObject trailRenderObject;

    [Space] [Header("Particles")] [SerializeField]
    private GameObject lightningParticle;
    
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
   

    [Space] [Header("Wall Jumping")] [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallMoveSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private float wallGravity;
    [SerializeField] private float maxWallDistance;
    [SerializeField] private bool wallCheckDisableInvoked;

    [SerializeField] [Space] [Header("Technical Data")] private float prevDir;
    [SerializeField] private bool jumpReleaseAvailable;
    [SerializeField] private bool secondJumpAvailable;
    [SerializeField] private bool secondJumpConsumed;
    [SerializeField] private float wallDirection;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool wallJumpEnabled;
    [SerializeField] private bool hasWallJumped;
    [SerializeField] private float wallTime;

    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        wallJumpEnabled = true;
        StartCoroutine(WallLightningEffect());
        StartCoroutine(RunLightningEffect());
    }

    private void Update()
    {
        velocity.x = ((playerState == PlayerState.Clinging ? wallMoveSpeed : moveSpeed) * moveInput.x);

        if (hasWallJumped && Math.Abs(moveInput.x - (-wallDirection)) < 0.1)
        {
            velocity.x *= 0.2f;
        }

        horizontalVelocityImpulse = Mathf.Lerp(horizontalVelocityImpulse, 0, impulseLerp * Time.deltaTime);

        RaycastHit groundHit;
        if (Physics.Raycast(groundCheckPositionL.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance)
            || Physics.Raycast(groundCheckPositionR.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance)
                || Physics.Raycast(groundCheckPositionM.position, transform.TransformDirection(Vector3.down), out groundHit, groundCheckMaxDistance))
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

        // Gravity.
        if (playerState == PlayerState.Airborne)
        {
            velocity.y += gravity * Time.deltaTime;

            if (velocity.y < 0)
            {
                trailRenderObject.SetActive(true);
            }
        }
        else
            trailRenderObject.SetActive(false);
        
        if (playerState == PlayerState.Clinging)
        {
            wallTime += Time.deltaTime;
            velocity.y += wallGravity * Time.deltaTime;
            secondJumpAvailable = false;
        }
        else
            wallTime = 0.0f;


        // Animation.

        if (velocity.x > 0f)
        {
            playerSprite.flipX = false;
        }

        if (velocity.x < 0f)
        {
            playerSprite.flipX = true;
        }
        
        playerAnimator.SetBool("Moving", moveInput.x != 0);
        playerAnimator.SetBool("Grounded", playerState == PlayerState.Grounded);
        playerAnimator.SetFloat("YVelocity", velocity.y);
        playerAnimator.SetFloat("WallTime", wallTime);
    }

    private void FixedUpdate()
    {
        Vector2 tempVelocity = new Vector2(Mathf.Lerp(rb.velocity.x, velocity.x, moveLerp * Time.deltaTime), velocity.y);

        rb.velocity = tempVelocity + new Vector2(horizontalVelocityImpulse * 50, 0f);
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
            
            playerState = PlayerState.Clinging;
        }
        else if (canWallJump && !wallCheckDisableInvoked)
        {
            Invoke(nameof(DisableWallJump), 0.2f);
            wallCheckDisableInvoked = true;
            playerState = PlayerState.Airborne;
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (Mathf.Abs(moveInput.x) > 0)
        {
            prevDir = moveInput.x;
        }
    }

    private void OnJump()
    {
        if (playerState == PlayerState.Grounded || (secondJumpAvailable && playerState != PlayerState.Clinging))
        {
            if (!secondJumpConsumed && !canWallJump)
            {
                velocity.y += jumpForce;

                Invoke(nameof(EnableSecondJump), 0.3f);
            }
            else
            {
                velocity.y = jumpForce;
                
                GameObject lightning = Instantiate(lightningParticle, ceilingCheckPosition.position + new Vector3(prevDir > 0 ? -0.15f : 0.15f, 0, -0.3f), 
                    Quaternion.Euler(new Vector3(0, 0, prevDir > 0 ? -120f : -60f)));
                
                GameObject lightning2 = Instantiate(lightningParticle, ceilingCheckPosition.position + new Vector3(prevDir > 0 ? 0.15f : -0.15f, 0, -0.3f), 
                    Quaternion.Euler(new Vector3(0, 0, prevDir > 0 ? -120f : -60f)));

                lightning.transform.localScale = new Vector3(0.15f, 0.1f, 0.1f);
                lightning2.transform.localScale = new Vector3(0.15f, 0.1f, 0.1f);
            }
            
            secondJumpAvailable = false;
            
            Invoke(nameof(EnableJumpRelease), 0.1f);
        }

        if (canWallJump)
        {
            velocity.y = wallJumpDirection.y * wallJumpForce;
            horizontalVelocityImpulse += (wallJumpDirection.x * -wallDirection) * wallJumpForce;
            
            wallJumpEnabled = false;
            canWallJump = false;
            hasWallJumped = true;
            secondJumpAvailable = false;
            
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

    private IEnumerator WallLightningEffect()
    {
        while (true)
        {
            if (playerState == PlayerState.Clinging)
            {
                GameObject lightning1 = Instantiate(lightningParticle,
                    transform.position + new Vector3(wallDirection * 0.6f, 0.75f, -0.25f),
                    Quaternion.Euler(new Vector3(0f, 0f, 270f)));

                GameObject lightning2 = Instantiate(lightningParticle,
                    transform.position + new Vector3(wallDirection * 0.6f, 1f, -0.25f),
                    Quaternion.Euler(new Vector3(0f, 0f, 90f)));

                lightning1.transform.localScale = new Vector3(0.06f, 0.125f, 0.1f);
                lightning2.transform.localScale = new Vector3(0.06f, 0.125f, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private IEnumerator RunLightningEffect()
    {
        while (true)
        {
            if (playerState == PlayerState.Grounded && velocity.x != 0)
            {
                GameObject lightning1 = Instantiate(lightningParticle,
                    transform.position + new Vector3(0f, 0.1f, -0.25f),
                    Quaternion.Euler(new Vector3(0f, 0f, prevDir < 0f ? 180f : 0f)));

                lightning1.transform.localScale = new Vector3(0.015f, 0.075f, 0.1f);
                
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
