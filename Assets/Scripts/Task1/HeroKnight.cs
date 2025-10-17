using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroContext : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    
    // Ссылки на UI кнопки
    [SerializeField] Button attack1Button;
    [SerializeField] Button attack2Button;
    [SerializeField] Button attack3Button;
    [SerializeField] EnemyManager enemyManager; // Добавлена ссылка на менеджер врагов

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    private AttackPerformer attackPerformer;
    private IAttackStrategy[] attackStrategies;
    private Color normalColor = Color.white;
    private Color accentColor = Color.yellow;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        // Инициализация системы атак
        attackPerformer = new AttackPerformer(m_animator);
        attackStrategies = new IAttackStrategy[]
        {
            new Attack1Strategy(),
            new Attack2Strategy(),
            new Attack3Strategy()
        };

        SetupAttackButtons();
        SetActiveStrategy(0); // Установка первой стратегии по умолчанию
    }

    void Update()
    {
        // Увеличиваем таймер, контролирующий комбо атаки
        float prevTimeSinceAttack = m_timeSinceAttack;
        m_timeSinceAttack += Time.deltaTime;

        // Увеличиваем таймер, проверяющий длительность кувырка
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Отключаем кувырок, если таймер превышает длительность
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // Проверка земли
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Обработка движения и физики
        HandleMovement();
        
        // Обработка атаки по Q
        if (Input.GetKeyDown(KeyCode.Q) && !m_rolling && m_timeSinceAttack > 0.25f)
        {
            attackPerformer.PerformAttack();
            m_timeSinceAttack = 0.0f;
        }

        // Остальная логика...
        HandleOtherInput();
        HandleAnimations();
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
    }

    private void HandleOtherInput()
    {
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || 
                         (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        // Смерть
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
        // Получение урона
        else if (Input.GetKeyDown("q") && !m_rolling)
        {
            m_animator.SetTrigger("Hurt");
        }
        // Блок
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
        }
        // Кувырок
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
        // Прыжок
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
    }

    private void HandleAnimations()
    {
        float inputX = Input.GetAxis("Horizontal");
        
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    private void SetupAttackButtons()
    {
        attack1Button.onClick.AddListener(() => SetActiveStrategy(0));
        attack2Button.onClick.AddListener(() => SetActiveStrategy(1));
        attack3Button.onClick.AddListener(() => SetActiveStrategy(2));
    }

    private void SetActiveStrategy(int index)
    {
        if (index >= 0 && index < attackStrategies.Length)
        {
            attackPerformer.SetStrategy(attackStrategies[index]);
            UpdateButtonColors(index);
            
            // Уведомляем менеджер врагов о смене типа атаки
            if (enemyManager != null)
            {
                enemyManager.SwitchEnemy(index);
            }
        }
    }

    private void UpdateButtonColors(int activeIndex)
    {
        var buttons = new Button[] { attack1Button, attack2Button, attack3Button };
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == activeIndex) ? accentColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    // Animation Events
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    // Добавлены недостающие переменные для совместимости с оригинальным кодом
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
}