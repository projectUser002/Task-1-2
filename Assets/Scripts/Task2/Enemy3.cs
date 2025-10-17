using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : MonoBehaviour, IEnemyBehavior
{
    private Animator animator;
    private Animator playerAnimator;
    private bool wasPlayerAttacking = false;

    public void Initialize(Transform playerTransform, Animator playerAnimator)
    {
        this.playerAnimator = playerAnimator;
    }

    public void UpdateBehavior()
    {

        bool isPlayerAttacking = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                                playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                                playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3");

        if (isPlayerAttacking && !wasPlayerAttacking)
        {
            animator.SetTrigger("Attack");
        }

        if (!isPlayerAttacking)
        {
            animator.SetInteger("AnimState", 0);
        }

        wasPlayerAttacking = isPlayerAttacking;
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
}
