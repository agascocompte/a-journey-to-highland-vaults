using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorFireAura : MonoBehaviour
{
    private PlayerHealth playerhealth;
    private bool playerInside = false;
    private float timer = 0f;
    private float timeBetweenTicks = 0.5f;

    public int damage = 4;

    void Start()
    {
        playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        // Incremento por nivel
        damage = (int)((float)damage * (1 + (Mathf.Pow(Configuration.currentLevel, 2) * Configuration.factorIncremento)));

    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (playerInside && timer > timeBetweenTicks)
        {
            playerhealth.TakeDamage(damage);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = false;
        }
    }
}
