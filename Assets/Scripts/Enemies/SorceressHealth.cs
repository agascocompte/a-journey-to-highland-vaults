using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SorceressHealth : MonoBehaviour
{
	public AudioClip thunderClip;
	private AudioSource audio;


	private bool isDead;
	private Animator anim;
	private float maxSufferingTime = 0.4f;
	private SorceressAttack sorceressAttack;
	private GameObject punishment;
	private bool inCooldown;
	private float cooldown = 0.3f;
	private float timer = 0f;

	public int health = 30;
	public int enemy_score = 70;

    public void Awake()
    {
		audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
	{
		sorceressAttack = GetComponent<SorceressAttack>();
		anim = GetComponentInChildren<Animator>();
		isDead = false;
		inCooldown = false;

		// Incrementos por nivel
		health = (int)((float)health * (1 + (Mathf.Pow(Configuration.currentLevel, 2) * Configuration.factorIncremento)));
		enemy_score = (int)((float)enemy_score * (1 + (Mathf.Pow(Configuration.currentLevel, 2) * Configuration.factorIncremento)));

		foreach (Transform child in transform)
		{
			if (child.name == "SimpleLightningBoltPrefab")
			{
				punishment = child.gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isDead)
		{
			maxSufferingTime -= Time.deltaTime;
			sorceressAttack.enabled = false;

			if (maxSufferingTime <= 0)
			{
				UpdateEnemyCounter();
				Destroy(this.gameObject);
			}
		}

		if (inCooldown)
		{
			timer += Time.deltaTime;

			if (timer > cooldown)
			{
				timer = 0;
				inCooldown = false;
			}
		}
	}

	private void UpdateEnemyCounter()
	{
		foreach (var item in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			if (item.CompareTag("Room"))
			{
				item.GetComponent<RoomObjectsSpawn>().updateEnemyCount();
				break;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PlayerProjectile" && !inCooldown)
		{
			health -= GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerShooting>().damagePerShot;

			if (health <= 0 && !isDead)
			{
				isDead = true;
				anim.SetBool("isDead", true);
				
				SendGodsPunish();

				ScoreManager.score += enemy_score;
			}

			inCooldown = true;
		}
	}

	void SendGodsPunish() 
	{
		audio.clip = thunderClip;
		audio.PlayOneShot(thunderClip, 0.7f);

		punishment.SetActive(true);
	}
}
