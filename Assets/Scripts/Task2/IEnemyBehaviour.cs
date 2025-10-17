using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior
{
    void Initialize(Transform playerTransform, Animator playerAnimator);
    void UpdateBehavior();
    void Spawn(Vector3 position);
    void Despawn();
}
