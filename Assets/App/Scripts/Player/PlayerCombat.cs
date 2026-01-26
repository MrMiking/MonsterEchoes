using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Inline] PlayerAttack[] attacks;

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
        if (currentAttackTimer > attacks[currentAttackId].attackTime + comboInputTime)
            OnComboEnd();
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (currentAttackId == -1)
        {
            currentAttackId = 0;
            currentAttackTimer = 0;
            visual.ComboAttack();
        }
        else if (currentAttackId >= attacks.Length - 1) ;
        else if (currentAttackTimer >= attacks[currentAttackId].attackTime - comboInputTime)
        {
            currentAttackId++;
            currentAttackTimer = 0;
            visual.ComboAttack();
        }
    }

    void OnComboEnd()
    {
        currentAttackId = -1;
        visual.StopCombo();
    }
}