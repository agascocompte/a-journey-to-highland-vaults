using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceressFireball : MonoBehaviour
{
    private PlayerHealth playerhealth;

    public int damage = 45;

    void Start()
    {
        playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        // Incremento por nivel
        damage = (int)((float)damage * (1 + (Mathf.Pow(Configuration.currentLevel, 2) * Configuration.factorIncremento)));
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerhealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            Destroy(this.gameObject);
        }
    }
}