using MVsToolkit.Dev;
using MVsToolkit.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PlayerAttack[] attacks;
    [SerializeField] PlayerAttack airAttack;

    [Space(10)]
    [SerializeField] float comboInputTime;

    bool canDoAirAttack = true;
    bool isAttacking = false;
    int currentAttackId;
    float currentAttackTimer;

    [Header("References")]
    [SerializeField] PlayerVisual visual;
    [SerializeField] PlayerMovement movement;
    [SerializeField] Rigidbody2D rb;

    [Foldout("Inputs")]
    [SerializeField] InputActionReference attackIA;

    private void OnEnable()
    {
        attackIA.action.Enable();

        attackIA.action.started += Attack;
    }

    private void OnDisable()
    {
        attackIA.action.started -= Attack;
    }

    private void Update()
    {
        if (!canDoAirAttack && movement.IsGrounded) canDoAirAttack = true;

        if (currentAttackId < 0) return;

        currentAttackTimer += Time.deltaTime;
        if (currentAttackTimer > attacks[currentAttackId].AttackTime + comboInputTime)
            OnComboEnd();
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (movement.IsGrounded)
        {
            if (currentAttackId == -1)
            {
                rb.linearVelocity = Vector2.zero;

                isAttacking = true;

                currentAttackId = 0;
                currentAttackTimer = 0;

                visual.SetComboAttack(true);
                visual.FirstComboAttack();

                attacks[currentAttackId].Attack(rb.position, visual.LookAtRight());
            }
            else if (currentAttackId >= attacks.Length - 1) return;
            else if (currentAttackTimer >= attacks[currentAttackId].AttackTime - comboInputTime)
            {
                rb.linearVelocity = Vector2.zero;

                currentAttackId++;
                currentAttackTimer = 0;
                visual.ComboAttack();

                attacks[currentAttackId].Attack(rb.position, visual.LookAtRight());
            }
        }
        else if(canDoAirAttack)
        {
            movement.StopDashTime();
            rb.linearVelocity = Vector2.zero;

            isAttacking = true;
            canDoAirAttack = false;

            visual.AirAttack();
            visual.SetAirAttack(true);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

            this.Delay(() =>
            {
                visual.SetAirAttack(false);
                isAttacking = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }, airAttack.AttackTime);

            airAttack.Attack(rb.position, visual.LookAtRight());
        }        
    }

    public void ResetAirAttack()=>canDoAirAttack = true;

    public void OnComboEnd()
    {
        currentAttackId = -1;
        visual.SetComboAttack(false);
        isAttacking = false;
    }

    public bool IsAttacking() => isAttacking;

    private void OnDrawGizmosSelected()
    {
        foreach (PlayerAttack attack in attacks)
        {
            DrawCapsuleGizmo(rb.position + attack.pos, attack.size, attack.debugColor);
        }
    }

    void DrawCapsuleGizmo(Vector2 center, Vector2 size, Color color)
    {
        Gizmos.color = color;

        float radius = Mathf.Min(size.x, size.y) * 0.5f;
        float height = Mathf.Max(size.x, size.y);
        float cylinderLength = height - radius * 2f;

        bool vertical = size.y > size.x;

        if (vertical)
        {
            Vector2 top = center + Vector2.up * (cylinderLength * 0.5f);
            Vector2 bottom = center + Vector2.down * (cylinderLength * 0.5f);

            Gizmos.DrawLine(top + Vector2.left * radius, bottom + Vector2.left * radius);
            Gizmos.DrawLine(top + Vector2.right * radius, bottom + Vector2.right * radius);

            DrawCircleGizmo(top, radius);
            DrawCircleGizmo(bottom, radius);
        }
        else
        {
            Vector2 right = center + Vector2.right * (cylinderLength * 0.5f);
            Vector2 left = center + Vector2.left * (cylinderLength * 0.5f);

            Gizmos.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius);
            Gizmos.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius);

            DrawCircleGizmo(left, radius);
            DrawCircleGizmo(right, radius);
        }
    }
    void DrawCircleGizmo(Vector2 center, float radius, int segments = 24)
    {
        float step = 360f / segments;
        Vector3 prev = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * step);
            Vector3 next = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }

}