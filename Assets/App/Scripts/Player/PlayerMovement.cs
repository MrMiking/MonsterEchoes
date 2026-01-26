using UnityEngine;
using MVsToolkit.Dev;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Space(5)]
    [SerializeField] float dashForce;
    [SerializeField] float dashTime = .3f;
    [SerializeField] float dashCooldown = .8f;

    [Space(10)]
    [SerializeField] Vector2 groundCheckPosOffset;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    Vector2 moveInput;

    bool isDashing;
    bool canDash = true;

    bool isGrounded;

    [Foldout("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVisual visual;

    [Foldout("Inputs")]
    [SerializeField] InputActionReference moveInputIA;
    [SerializeField] InputActionReference jumpInputIA;
    [SerializeField] InputActionReference dashInputIA;

    private void OnEnable()
    {
        jumpInputIA.action.started += Jump;
        dashInputIA.action.started += Dash;
    }
    private void OnDisable()
    {
        jumpInputIA.action.started -= Jump;
        dashInputIA.action.started -= Dash;
    }

    private void FixedUpdate()
    {
        moveInput = moveInputIA.action.ReadValue<Vector2>();
        moveInput.y = 0;

        isGrounded = IsGrounded();

        if(!isDashing) Move(moveInput);

        visual.SetGrounded(isGrounded);
        visual.SetVerticalVelocity(rb.linearVelocityY);
        visual.FlipX(moveInput.x);
    }

    void Move(Vector2 input)
    {
        rb.AddForce(input * moveSpeed);
        visual.MoveAnim(input.x);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && !isDashing)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Dash(InputAction.CallbackContext ctx)
    {
        if (!canDash) return;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(moveInput * dashForce, ForceMode2D.Impulse);

        StartCoroutine(DashTime());
        StartCoroutine(DashCooldown());
    }
    IEnumerator DashTime()
    {
        visual.SetDash(true);
        isDashing = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        
        yield return new WaitForSeconds(dashTime);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        visual.SetDash(false);
        isDashing = false;
    }
    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
