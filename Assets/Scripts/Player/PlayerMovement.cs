using System;
using System.Collections;
using System.Collections.Generic;
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

    [Space] [SerializeField] private Vector2 velocity;
    
    [Space] [SerializeField] private float moveSpeed;
    [SerializeField] private float moveLerp;
    
    [Space] [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckMaxDistance;

    private void Update()
    {
        Vector2 tempVelocity = new Vector2(Mathf.Lerp(rb.velocity.x, velocity.x, moveLerp * Time.deltaTime), velocity.y);
       
        rb.velocity = tempVelocity;

        // Ground Check.
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheckPosition.position, Vector3.down, groundCheckMaxDistance))
            playerState = PlayerState.Grounded;

        // Gravity.
        if (playerState != PlayerState.Grounded && playerState != PlayerState.Clinging)
            velocity.y += gravity;
    }

    private void OnMove(InputValue value)
    {
        velocity.x = moveSpeed * value.Get<Vector2>().x * Time.deltaTime * 1000f;
    }

    private void OnJump()
    {
        if (playerState == PlayerState.Grounded)
        {
            velocity.y += jumpForce;
        }
    }
}
