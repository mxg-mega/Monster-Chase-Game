using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private PlayerControls inputs;
    private bool isJumping = false;

    private void Awake()
    {
        inputs = new PlayerControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputs.Player.Enable();
        inputs.Player.Jump.performed += OnJumpPerformed;
    }

    private void Update()
    {
        moveInput = inputs.Player.Move.ReadValue<Vector2>();
        moveInput.y = 0;
        isJumping = !IsGrounded();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 targetVelocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.05f);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Jumped!" + inputs.Player.Jump.ReadValue<float>());
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.yellow);
        return hit.collider != null;
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
        inputs.Player.Jump.performed -= OnJumpPerformed;
    }
}
