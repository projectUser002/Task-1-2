using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : EnemyBase
{
    private bool hasAttacked = false;
    private bool isFirstSpawn = true;
    
    protected override bool CheckAttackCondition()
    {
        return isFirstSpawn && !hasAttacked;
    }
    
    protected override void PerformAttack()
    {
        animator.SetTrigger("Attack");
        hasAttacked = true;
        Debug.Log($"Enemy1 выполнил одноразовую атаку");
    }
    
    protected override void PerformIdle()
    {
    }
    
    protected override void AdditionalBehavior()
    {
    }
    
    public override string GetEnemyType()
    {
        return "Type1 - Одноразовая атака";
    }
    
    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        isFirstSpawn = true;
        hasAttacked = false;
    }
    
    public override void Despawn()
    {
        base.Despawn();
        isFirstSpawn = false;
    }
}