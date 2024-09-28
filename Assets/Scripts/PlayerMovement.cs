using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _grounded = true;

    [SerializeField] private Vector2 movementSpeed = new Vector2(1.8f, 5f);

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
    }

    private void UpdatePlayerMovement()
    {
        Vector2 velocity = new Vector2(0, _rb.velocity.y);
        //Handle horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        velocity.x = horizontalInput * movementSpeed.x;

        //Handle vertical movement
        bool jumpKey = Input.GetKey(KeyCode.Space);
        if (jumpKey && _grounded)
        {
            _grounded = false;
            velocity.y = movementSpeed.y;
        }

        _rb.velocity = velocity;
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