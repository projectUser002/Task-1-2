using UnityEngine;

public class AttackPerformer
{
    private IAttackStrategy currentStrategy;
    private Animator animator;

    public AttackPerformer(Animator animator)
    {
        this.animator = animator;
        currentStrategy = new Attack1Strategy();
    }

    public void SetStrategy(IAttackStrategy strategy)
    {
        currentStrategy = strategy;
    }

    public void PerformAttack()
    {
        if (currentStrategy != null)
        {
            currentStrategy.Attack(animator);
        }
    }
}