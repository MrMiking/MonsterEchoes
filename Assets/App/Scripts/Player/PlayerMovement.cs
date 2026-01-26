using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Space(10)]
    [SerializeField] Vector2 groundCheckPosOffset;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    [Foldout("References")]
    [SerializeField] Rigidbody2D rb;

    [Foldout("Inputs")]
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference jumpInput;

    private void OnEnable()
    {
        jumpInput.action.started += Jump;
    }
    private void OnDisable()
    {
        jumpInput.action.started -= Jump;
    }

    private void FixedUpdate()
    {
        Move(moveInput.action.ReadValue<Vector2>());
    }

    void Move(Vector2 input)
    {
        input.y = 0;
        rb.AddForce(input * moveSpeed);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (IsGrounded())
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            (Vector2)transform.position + groundCheckPosOffset,
            groundCheckRadius,
            groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosOffset, groundCheckRadius);
    }
}
