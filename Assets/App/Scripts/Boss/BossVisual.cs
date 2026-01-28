using MVsToolkit.Dev;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum BossColor
{
    Red,
    Purple,
    Green,
}

public class BossVisual : MonoBehaviour
{
    [Header("Settings")]
    [ColorUsage(true, true)][SerializeField] private Color defaultColor;

    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] private RSO_BossColor rsoBossColor;
    [SerializeField] private Material bossMaterial;

    [Header("Colors")]
    [ColorUsage(true, true)][SerializeField] private Color redColor;
    [ColorUsage(true, true)][SerializeField] private Color purpleColor;
    [ColorUsage(true, true)][SerializeField] private Color greenColor;
    
    [Foldout("Atk 1")]
    public Action OnAttack1Dmg;
    
    [Foldout("Atk 2")]
    public Action OnAttack2Dmg1;
    public Action OnAttack2Dmg2;

    [Foldout("Atk 3")]
    public Action OnAttack3Dmg1;
    public Action OnAttack3Dmg2;
    public Action OnAttack3Dmg3;

    private void Start()
    {
        SetColor(rsoBossColor.Value);
    }

    private void SetColor(int color)
    {
        Color newColor = redColor;
        
        switch (color)
        {
            case 1:
                newColor = redColor;
                break;
            case 2:
                newColor = purpleColor;
                break;
            case 3:
                newColor = greenColor;
                break;
        }
        
        bossMaterial.SetColor("_Color", newColor);
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