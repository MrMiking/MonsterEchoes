using MVsToolkit.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer graphics;

    public void SetGrounded(bool isGrounded)
    {
        anim.SetBool("IsGrounded", isGrounded);
    }

    public void SetVerticalVelocity(float yVelocity)
    {
        anim.SetFloat("VerticalVelocity", yVelocity);
    }

    public void MoveAnim(float horizontalInput)
    {
        anim.SetFloat("HorizontalInput", Mathf.Abs(horizontalInput));
    }

    public void FlipX(float horizontalInput)
    {
        if (horizontalInput > 0) graphics.flipX = false;
        else if (horizontalInput < 0) graphics.flipX = true;
    }

    public void SetDash(bool isDashing)
    {
        if (isDashing) anim.SetTrigger("Dash");
        anim.SetBool("IsDashing", isDashing);
    }

    public void FirstComboAttack()
    {
        anim.SetTrigger("FirstComboAttack");
    }
    public void ComboAttack()
    {
        anim.SetTrigger("ComboAttack");
    }
    public void SetComboAttack(bool isOnCombo)
    {
        anim.SetBool("OnComboAttack", isOnCombo);
    }
}