using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : MonoBehaviour, IEnemyBehavior
{
    private Animator animator;
    private bool hasAttacked = false;

    public void Initialize(Transform playerTransform, Animator playerAnimator)
    {

    }

    public void UpdateBehavior()
    {

    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        animator = GetComponent<Animator>();
        
        if (!hasAttacked)
        {
            animator.SetTrigger("Attack");
            hasAttacked = true;
        }
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}