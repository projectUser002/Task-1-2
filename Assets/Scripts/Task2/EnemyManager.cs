using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject[] enemyPrefabs; // 0: Type1, 1: Type2, 2: Type3
    
    private IEnemyBehavior currentEnemy;
    private GameObject currentEnemyObject;
    private int currentEnemyIndex = -1;

    void Update()
    {
        if (currentEnemy != null)
        {
            currentEnemy.UpdateBehavior();
        }
    }

    public void SwitchEnemy(int enemyIndex)
    {
        if (enemyIndex == currentEnemyIndex) return;

        // Удаляем текущего врага
        if (currentEnemy != null)
        {
            currentEnemy.Despawn();
            currentEnemy = null;
            currentEnemyObject = null;
        }

        currentEnemyIndex = enemyIndex;
        
        // Создаем нового врага справа от игрока
        Vector3 spawnPosition = playerTransform.position + Vector3.right * 3f;
        currentEnemyObject = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        currentEnemy = currentEnemyObject.GetComponent<IEnemyBehavior>();
        
        if (currentEnemy != null)
        {
            currentEnemy.Initialize(playerTransform, playerAnimator);
            currentEnemy.Spawn(spawnPosition);
        }
    }
}