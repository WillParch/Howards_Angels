using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;

    private Rigidbody rb;

    void Start()
    {
        // Get the rigidbody component attached to the projectile
        rb = GetComponent<Rigidbody>();

        // Set the velocity of the projectile in the forward direction
        rb.velocity = transform.forward * speed;

        // Destroy the projectile after 2 seconds
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
