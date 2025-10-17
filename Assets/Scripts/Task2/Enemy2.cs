using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : MonoBehaviour, IEnemyBehavior
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1.5f;
    
    private Animator animator;
    private float attackTimer = 0f;
    private Transform playerTransform;

    public void Initialize(Transform playerTransform, Animator playerAnimator)
    {
        this.playerTransform = playerTransform;
    }

    public void UpdateBehavior()
    {
        attackTimer += Time.deltaTime;
        
        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        animator = GetComponent<Animator>();
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            Vector3 shootDirection = (playerTransform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(shootDirection);
            }
        }
    }
}