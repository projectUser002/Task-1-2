using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;
    
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        rb.velocity = direction * speed;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        
        // Поворачиваем снаряд в направлении движения
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Наносим урон игроку
            Destroy(gameObject);
        }
    }
}