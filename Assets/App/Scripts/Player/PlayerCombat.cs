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

                attacks[currentAttackId].Attack(visual.LookAtRight());
            }
            else if (currentAttackId >= attacks.Length - 1) return;
            else if (currentAttackTimer >= attacks[currentAttackId].AttackTime - comboInputTime)
            {
                rb.linearVelocity = Vector2.zero;

                currentAttackId++;
                currentAttackTimer = 0;
                visual.ComboAttack();

                attacks[currentAttackId].Attack(visual.LookAtRight());
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

            airAttack.Attack(visual.LookAtRight());
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
}