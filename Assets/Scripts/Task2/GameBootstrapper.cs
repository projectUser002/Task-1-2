using UnityEngine;
using UnityEngine.UI;

public class GameBootstrapper : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator playerAnimator;
    
    [Header("UI References")]
    [SerializeField] private Button attack1Button;
    [SerializeField] private Button attack2Button;
    [SerializeField] private Button attack3Button;
    
    [Header("Enemy System")]
    [SerializeField] private EnemyManager enemyManager;
    
    private AttackPerformer attackPerformer;
    private IAttackStrategy[] attackStrategies;
    
    void Start()
    {
        
        if (playerTransform == null || playerAnimator == null)
        {
            Debug.LogError("Не назначены ссылки на игрока!");
            return;
        }
        
        if (enemyManager == null)
        {
            Debug.LogError("Не назначен EnemyManager!");
            return;
        }

        InitializeAttackSystem();

        InitializeEnemySystem();
        
        SetupUI();
    }
    
    private void InitializeAttackSystem()
    {
        attackPerformer = new AttackPerformer(playerAnimator);
        attackStrategies = new IAttackStrategy[]
        {
            new Attack1Strategy(),
            new Attack2Strategy(),
            new Attack3Strategy()
        };
    }
    
    private void InitializeEnemySystem()
    {
        if (enemyManager != null)
        {
            Debug.Log("EnemyManager инициализирован");
        }
    }
    
    private void SetupUI()
    {
        if (attack1Button != null)
            attack1Button.onClick.AddListener(() => OnAttackTypeSelected(0));
        
        if (attack2Button != null)
            attack2Button.onClick.AddListener(() => OnAttackTypeSelected(1));
        
        if (attack3Button != null)
            attack2Button.onClick.AddListener(() => OnAttackTypeSelected(2));
    }
    
    private void OnAttackTypeSelected(int index)
    {
        if (index < 0 || index >= attackStrategies.Length)
            return;
        
        attackPerformer.SetStrategy(attackStrategies[index]);
        if (enemyManager != null)
        {
            enemyManager.SwitchToEnemy(index);
        }
        
        Debug.Log($"Выбран тип атаки: {index + 1}");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            attackPerformer.PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnAttackTypeSelected(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnAttackTypeSelected(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnAttackTypeSelected(2);
        }
        

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (enemyManager != null)
            {
                enemyManager.SwitchEnemy(0);
            }
        }
    }
}