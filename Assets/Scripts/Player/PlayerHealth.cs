using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private Slider healthBar;
    private Image damageImage;
    private Image shieldImage;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    public int shield = 0;

    public AudioClip hurtClip;
    public AudioClip deadClip;
    public AudioClip potionClip;

    private AudioSource audio;

    private GameObject mainCamera;

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("GameMusic");

        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;

        isDead = false;
        damaged = false;

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        damageImage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<Image>();
        shieldImage = GameObject.FindGameObjectWithTag("ShieldImage").GetComponent<Image>();

        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        //Debug.Log(currentHealth);
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        //Debug.Log("Vida: " + currentHealth);
        //Debug.Log("Escudo: " + shield);
    }

    public void GainHealth(int amount)
    {
        audio.PlayOneShot(potionClip, 0.9f);

        currentHealth += amount;

        if (currentHealth > startingHealth)
        { 
            currentHealth = startingHealth;
        }

        healthBar.value = (float)currentHealth;
    }

    public void GainShield(int amount)
    {
        audio.PlayOneShot(potionClip, 0.9f);

        shield = amount;

        shieldImage.enabled = true;
    }

    public void TakeDamage(int amount)
    {
        audio.PlayOneShot(hurtClip, 0.9f);
        damaged = true;
        if (shield > 0)
        {
            shield -= amount;
            if (shield < 0)
            {
                currentHealth += shield;
                shield = 0;
                shieldImage.enabled = false;
            }
        }
        else
        {
            currentHealth -= amount;
        }

        healthBar.value = (float) currentHealth;

        //playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        mainCamera.GetComponent<AudioSource>().Stop();
        audio.clip = deadClip;
        audio.PlayOneShot(deadClip, 0.9f);

        isDead = true;

        GetComponentInChildren<DeathTransition>().activateDeathTransition();

        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }
}
