using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject deathTrigger; // Reference to the invisible game object
    public int enemyHealth = 1; // Health of the enemy
    public int playerDamage = 1; // Damage dealt to the player on collision

    public void TakeDamage()
    {
        enemyHealth--;

        if (enemyHealth <= 0)
        {
            Destroy(gameObject); // Destroy the enemy
        }
    }
}