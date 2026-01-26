using UnityEngine;

public class BossVisual : MonoBehaviour
{
    //[Header("Settings")]
    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer graphics;
    
    //[Header("Input")]
    //[Header("Output")]

    public void SetMoveXInput(float input)
    {
        anim.SetFloat("HorizontalInput", Mathf.Abs(input));
    }

    public void FlipX(float horizontalInput)
    {
        if (horizontalInput > 0) graphics.flipX = false;
        else if (horizontalInput < 0) graphics.flipX = true;
    }

    public void Attack1()
    {
        anim.SetTrigger("Attack1");
    }
}