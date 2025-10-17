using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public interface IAttackStrategy
{
    void Attack(Animator animator);
}

public class Attack1Strategy : IAttackStrategy
{
    public void Attack(Animator animator)
    {
        animator.SetTrigger("Attack1");
    }
}

public class Attack2Strategy : IAttackStrategy
{
    public void Attack(Animator animator)
    {
        animator.SetTrigger("Attack2");
    }
}

public class Attack3Strategy : IAttackStrategy
{
    public void Attack(Animator animator)
    {
        animator.SetTrigger("Attack3");
    }
}
