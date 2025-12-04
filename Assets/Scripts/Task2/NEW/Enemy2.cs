using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Враг №2: Постоянно атакует с выстрелами
public class EnemyType2 : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private Transform firePoint;
    
    private float attackTimer = 0f;
    
    protected override bool CheckAttackCondition()
    {
        return true;
    }
    
    protected override void PerformAttack()
    {
        attackTimer += Time.deltaTime;
        
        if (attackTimer >= attackInterval)
        {

            animator.SetTrigger("Attack");

            ShootProjectile();
            
            attackTimer = 0f;
        }
    }
    
    protected override void PerformIdle()
    {

        if (attackTimer > attackInterval * 0.5f)
        {
            animator.SetTrigger("Idle");
        }
    }
    
    private void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Vector3 shootDirection = (playerTransform.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(shootDirection);
            }
            
            Debug.Log($"Enemy2 выполнил выстрел в направлении: {shootDirection}");
        }
    }
    
    public override string GetEnemyType()
    {
        return "Type2 - Постоянная стрельба";
    }
}