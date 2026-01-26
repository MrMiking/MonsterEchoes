using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    [Header("References")]
    public float AttackTime;

    [Space(5)]
    [SerializeField] float attackDashForce;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    public virtual void Attack(bool right = true) 
    {
        rb.AddForce((right ? Vector2.right : Vector2.left) * attackDashForce, ForceMode2D.Impulse);
    }
}