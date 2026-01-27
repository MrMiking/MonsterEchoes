using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BossVisual visual;
    
    //[Header("Input")]
    //[Header("Output")]

    public void Move(Vector2 input)
    {
        rb.AddForce(input * moveSpeed);

        visual.FlipX(input.x);
        visual.SetMoveXInput(input.x);
    }

    public void TpToo(Vector2 position)
    {
        rb.position = position;
        rb.linearVelocity = Vector2.zero;
    }
}