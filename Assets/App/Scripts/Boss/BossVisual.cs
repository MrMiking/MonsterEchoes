using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.Events;

public class BossVisual : MonoBehaviour
{
    [Header("Settings")]
    [ColorUsage(true, true)][SerializeField] private Color defaultColor;

    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] private RSE_SetColor rseSetColor;
    [SerializeField] private Material bossMaterial;
    
    [Foldout("Atk 1")]
    [SerializeField] UnityEvent OnAttack1Dmg;
    
    [Foldout("Atk 2")]
    [SerializeField] UnityEvent OnAttack2Dmg1;
    [SerializeField] UnityEvent OnAttack2Dmg2;

    [Foldout("Atk 3")]
    [SerializeField] UnityEvent OnAttack3Dmg1;
    [SerializeField] UnityEvent OnAttack3Dmg2;
    [SerializeField] UnityEvent OnAttack3Dmg3;

    private void OnEnable()
    {
        rseSetColor.Action += SetColor;
    }

    private void OnDisable()
    {
        rseSetColor.Action -= SetColor;
    }

    private void Start()
    {
        SetColor(defaultColor);
    }

    private void SetColor(Color color)
    {
        bossMaterial.SetColor("_Color", color);
    }

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

    public void Attack2()
    {
        anim.SetTrigger("Attack2");
    }

    public void Attack3()
    {
        anim.SetTrigger("Attack3");
    }

    public void HandleAttack1Dmg() => OnAttack1Dmg?.Invoke();
    public void HandleAttack2Dmg1() => OnAttack2Dmg1?.Invoke();
    public void HandleAttack2Dmg2() => OnAttack2Dmg2?.Invoke();
    public void HandleAttack3Dmg1() => OnAttack3Dmg1?.Invoke();
    public void HandleAttack3Dmg2() => OnAttack3Dmg2?.Invoke();
    public void HandleAttack3Dmg3() => OnAttack3Dmg3?.Invoke();
}