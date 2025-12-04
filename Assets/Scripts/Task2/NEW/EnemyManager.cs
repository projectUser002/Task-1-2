using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPool
    {
        public EnemyBase enemyPrefab;
        public int poolSize = 3;
        [HideInInspector] public List<EnemyBase> pooledEnemies = new List<EnemyBase>();
    }
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private List<EnemyPool> enemyPools = new List<EnemyPool>();
    [SerializeField] private Vector3 spawnOffset = new Vector3(3f, 0f, 0f);
    
    private EnemyBase currentEnemy = null;
    private int currentEnemyIndex = -1;
    
    void Start()
    {
        InitializePools();
        
        // Спавним первого врага
        SwitchToEnemy(0);
    }
    
    void Update()
    {
        if (currentEnemy != null)
        {
            // Выполняем шаблонный метод для текущего врага
            currentEnemy.ExecuteBehavior();
        }
    }
    
    private void InitializePools()
    {
        foreach (var pool in enemyPools)
        {
            pool.pooledEnemies = new List<EnemyBase>();
            
            for (int i = 0; i < pool.poolSize; i++)
            {
                EnemyBase enemy = Instantiate(pool.enemyPrefab);
                enemy.Initialize(playerTransform, playerAnimator);
                enemy.Despawn();
                pool.pooledEnemies.Add(enemy);
                
                Debug.Log($"Инициализирован враг: {enemy.GetEnemyType()}");
            }
        }
    }
    
    public void SwitchToEnemy(int enemyIndex)
    {
        if (enemyIndex == currentEnemyIndex || enemyIndex < 0 || enemyIndex >= enemyPools.Count)
            return;

        if (currentEnemy != null)
        {
            currentEnemy.Despawn();
        }
        
        currentEnemyIndex = enemyIndex;
        
        // Получаем врага из пула
        EnemyPool targetPool = enemyPools[enemyIndex];
        EnemyBase availableEnemy = null;
        
        foreach (var enemy in targetPool.pooledEnemies)
        {
            if (!enemy.gameObject.activeSelf)
            {
                availableEnemy = enemy;
                break;
            }
        }
        
        if (availableEnemy == null)
        {
            availableEnemy = Instantiate(targetPool.enemyPrefab);
            availableEnemy.Initialize(playerTransform, playerAnimator);
            targetPool.pooledEnemies.Add(availableEnemy);
        }
        
        // Спавним врага
        Vector3 spawnPosition = playerTransform.position + spawnOffset;
        availableEnemy.Spawn(spawnPosition);
        currentEnemy = availableEnemy;
        
        Debug.Log($"Сменен враг на: {currentEnemy.GetEnemyType()}");
    }
    
    public void SwitchEnemy(int ind)
    {
        int nextIndex = (currentEnemyIndex + 1) % enemyPools.Count;
        SwitchToEnemy(nextIndex);
    }

    public EnemyBase GetCurrentEnemy()
    {
        return currentEnemy;
    }
}
