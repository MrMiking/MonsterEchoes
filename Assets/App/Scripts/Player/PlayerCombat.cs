using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PlayerAttack[] attacks;

    [Space(10)]
    [SerializeField] float comboInputTime;

    int currentAttackId;
    float currentAttackTimer;

    [Header("References")]
    [SerializeField] PlayerVisual visual;

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
        if (currentAttackId < 0) return;

        currentAttackTimer += Time.deltaTime;
        if (currentAttackTimer > attacks[currentAttackId].AttackTime + comboInputTime)
            OnComboEnd();
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (currentAttackId == -1)
        {
            currentAttackId = 0;
            currentAttackTimer = 0;
            visual.SetComboAttack(true);
            visual.FirstComboAttack();

            attacks[currentAttackId].Attack();
        }
        else if (currentAttackId >= attacks.Length - 1) ;
        else if (currentAttackTimer >= attacks[currentAttackId].AttackTime - comboInputTime)
        {
            currentAttackId++;
            currentAttackTimer = 0;
            visual.ComboAttack();

            attacks[currentAttackId].Attack();
        }
    }

    void OnComboEnd()
    {
        currentAttackId = -1;
        visual.SetComboAttack(false);
    }
}