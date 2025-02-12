using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
	public AudioClip shootClip;
	public AudioClip boostClip;

	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.85f;
	public float range = 100f;
	public float arrowVelocity = 100f;
	public GameObject arrow;
	public bool isTriple = false;

	private int baseDamagePerShot;
	private float baseTimeBetweenBullets;
	private AudioSource audio;

	Animator anim;
	float timer;
	Ray shootRay = new Ray();
	float camRayLength = 100f;
	int floorMask;
	//AudioSource gunAudio;
	GameObject player;


	void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		player = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponentInParent<Animator>();
		baseDamagePerShot = damagePerShot;
		baseTimeBetweenBullets = timeBetweenBullets;
		audio = GetComponent<AudioSource>();
		//gunAudio = GetComponent<AudioSource>();
	}    

	void Update()
	{
		if (player == null) player = GameObject.FindGameObjectWithTag("Player");
		timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
		{
			Shoot();			
		}
	}


	void Shoot()
	{
		timer = 0f;

		anim.SetBool("IsShooting", true);
		//gunAudio.Play();        

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;            
			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

			player.GetComponent<PlayerMovement>().Shoot(newRotation);

			// instantiate arrow
			GameObject newArrow = Instantiate(arrow, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
			newArrow.transform.Rotate(0f, 90f, 5f);
			newArrow.GetComponent<Rigidbody>().velocity = (floorHit.point - transform.position).normalized * arrowVelocity;

			if (isTriple)
			{
				Vector3 vector_triple = floorHit.point - transform.position;
				int angle = 15;

				Vector3 vector_triple_left = Quaternion.AngleAxis(-angle, Vector3.up) * vector_triple;
				Vector3 vector_triple_right = Quaternion.AngleAxis(angle, Vector3.up) * vector_triple;

				GameObject newArrowRight = Instantiate(arrow, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
				newArrowRight.transform.Rotate(0f, 90f+angle, 5f);
				newArrowRight.GetComponent<Rigidbody>().velocity = (vector_triple_right).normalized * arrowVelocity;

				GameObject newArrowLeft = Instantiate(arrow, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
				newArrowLeft.transform.Rotate(0f, 90f-angle, 5f);
				newArrowLeft.GetComponent<Rigidbody>().velocity = (vector_triple_left).normalized * arrowVelocity;
			}
		}
		audio.PlayOneShot(shootClip, 0.7f);
	}

    public void ResetPowerUps()
    {
		isTriple = false;
		damagePerShot = baseDamagePerShot;
		timeBetweenBullets = baseTimeBetweenBullets;

		GameObject.FindGameObjectWithTag("TripleArrowImage").GetComponent<Image>().enabled = false;
		GameObject.FindGameObjectWithTag("SpeedArrowImage").GetComponent<Image>().enabled = false;
		GameObject.FindGameObjectWithTag("DamageArrowImage").GetComponent<Image>().enabled = false;
	}

	public void ApplyMoreSpeed(float amount)
	{
		audio.clip = boostClip;
		audio.PlayOneShot(boostClip, 0.7f);
		timeBetweenBullets = amount;
	}

	public void ApplyMoreDamage(int amount)
	{
		audio.clip = boostClip;
		audio.PlayOneShot(boostClip, 0.7f);
		damagePerShot = amount;
	}

	public void ApplyTripleShot()
	{
		audio.clip = boostClip;
		audio.PlayOneShot(boostClip, 0.7f);
		isTriple = true;
	}
}
