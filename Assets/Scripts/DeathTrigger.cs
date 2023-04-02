using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public int projectileDamage = 1; // Damage dealt to the enemy by the projectile
    public EnemyController enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.TakeDamage(projectileDamage);
        }
    }
}
