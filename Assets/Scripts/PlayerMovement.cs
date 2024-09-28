using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public MapBounder mapBounder;
    [SerializeField] private Vector2 movementSpeed = new Vector2(1.8f, 5f);
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _grounded = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        UpdatePlayerMovement();
        UpdatePlayerScaling();
        UpdateAnimatorParameters();
        UpdatePlayerPosition();
    }

    private void UpdatePlayerMovement()
    {
        Vector2 velocity = new Vector2(0, _rb.velocity.y);
        //Handle horizontal movement
        velocity.x = GetHorizontalMovement();
        //Handle vertical movement
        bool jumpKey = Input.GetKey(KeyCode.Space);
        if (jumpKey && _grounded)
        {
            _grounded = false;
            velocity.y = movementSpeed.y;
        }

        _rb.velocity = velocity;
    }

    private float GetHorizontalMovement()
    {
        Vector2 maxBounds = mapBounder.GetMaxBounds();
        Vector2 minBounds = mapBounder.GetMinBounds();
        float currentXPosition = transform.position.x;
        float horizontalInput = Input.GetAxis("Horizontal");

        if (currentXPosition <= minBounds.x + 0.2f && horizontalInput < 0.01f)
            return -0.02f;
        
        if (currentXPosition >= maxBounds.x - 0.2f && horizontalInput > 0.01f)
            return 0.02f;
           
        return horizontalInput * movementSpeed.x;
    }

    private void UpdatePlayerScaling()
    {
        Vector3 scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Vector2 currentVelocity = _rb.velocity;
        //Handle player flip on x-axis
        if (currentVelocity.x > 0.01f)
            scale.x = -1;
        else if (currentVelocity.x < -0.01f)
            scale.x = 1;

        transform.localScale = scale;
    }

    private void UpdatePlayerPosition()
    {
        //Confine the player to the map if a map bounder is passed
        if (!mapBounder)
            return;

        Vector2 maxBounds = mapBounder.GetMaxBounds();
        Vector2 minBounds = mapBounder.GetMinBounds();
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + 0.03f, maxBounds.x - 0.03f);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void UpdateAnimatorParameters()
    {
        Vector2 currentVelocity = _rb.velocity;
        _animator.SetBool("isWalking", Math.Abs(currentVelocity.x) > 0.01f);
        _animator.SetBool("isFalling", !_grounded);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _grounded = true;
    }
}