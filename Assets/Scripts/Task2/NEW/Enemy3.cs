using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : EnemyBase
{
    private bool isPlayerAttacking = false;
    private bool wasPlayerAttacking = false;
    
    protected override bool CheckAttackCondition()
    {
        isPlayerAttacking = IsPlayerAttacking();

        bool shouldAttack = !wasPlayerAttacking && isPlayerAttacking;
        wasPlayerAttacking = isPlayerAttacking;
        
        return shouldAttack;
    }
    
    protected override void PerformAttack()
    {
        animator.SetTrigger("Attack");
        Debug.Log($"Enemy3 выполнил синхронную атаку с игроком");
    }
    
    protected override void PerformIdle()
    {
        if (!isPlayerAttacking)
        {
            animator.SetTrigger("Idle");
        }
    }
    
    private bool IsPlayerAttacking()
    {
        if (playerAnimator == null)
            return false;

        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Attack1") || 
               stateInfo.IsName("Attack2") || 
               stateInfo.IsName("Attack3");
    }
    
    public override string GetEnemyType()
    {
        return "Type3 - Синхронная атака";
    }
}
