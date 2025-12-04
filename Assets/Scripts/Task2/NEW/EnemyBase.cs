using UnityEngine;

// Базовый абстрактный класс, определяющий шаблон поведения врага
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected Animator playerAnimator;
    
    protected bool isInitialized = false;
    protected Vector3 spawnPosition;
    

    public void ExecuteBehavior()
    {
        if (!isInitialized)
            return;
            
        if (CheckAttackCondition())
        {
            PerformAttack();
        }
        else
        {
            PerformIdle();
        }
        AdditionalBehavior();
    }
    
    protected abstract bool CheckAttackCondition();
    protected abstract void PerformAttack();
    protected abstract void PerformIdle();
    
    protected virtual void AdditionalBehavior()
    {
        
    }

    public virtual void Initialize(Transform playerTransform, Animator playerAnimator)
    {
        this.playerTransform = playerTransform;
        this.playerAnimator = playerAnimator;
        this.animator = GetComponent<Animator>();
        isInitialized = true;
    }
    
    public virtual void Spawn(Vector3 position)
    {
        spawnPosition = position;
        transform.position = position;
        gameObject.SetActive(true);
        animator.SetTrigger("Idle");
    }
    
    public virtual void Despawn()
    {
        gameObject.SetActive(false);
    }

    public abstract string GetEnemyType();
}