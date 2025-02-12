using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    public int healthApplied = 25;

    PlayerHealth playerHealth;

    void Awake()
    {

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerHealth.currentHealth < playerHealth.startingHealth)
            {

                playerHealth.GainHealth(healthApplied);
                Destroy(this.gameObject);
            }            
        }
    }
}
