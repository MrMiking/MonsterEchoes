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

    public virtual void Attack() 
    {
        rb.AddForce(Vector2.right * attackDashForce, ForceMode2D.Impulse);
    }
}