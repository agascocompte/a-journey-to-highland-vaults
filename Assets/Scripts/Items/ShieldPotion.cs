using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPotion : MonoBehaviour
{
    public int shieldApplied = 25;

    PlayerHealth playerHealth;


    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            playerHealth.GainShield(shieldApplied);
            Destroy(this.gameObject);
        }
    }
}
