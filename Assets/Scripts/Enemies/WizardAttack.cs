using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardAttack : MonoBehaviour
{
    private Transform player;
    private Animator anim;
    //private Rigidbody rigidbody;
    private NavMeshAgent nav;

    public GameObject magicball;
    public float range = 7f;
    public float magicballVelocity = 6f;
    public float timeBetweenMagicballs = 1.2f;

    float startTime = 1f;
    float timer;

    public AudioClip attackClip;

    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        //rigidbody = GetComponent<Rigidbody>();
        timer = startTime;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float actual_distance = Vector3.Distance(transform.position, player.position);

        if (actual_distance <= range)
        {
            anim.SetBool("inRange", true);
            timer += Time.deltaTime;

            if (timer >= timeBetweenMagicballs) //&& Time.timeScale != 0)
            {
                RotateAndShoot();
            }
            nav.isStopped = true;
            nav.updateRotation = true;
        }
        else
        {
            timer = startTime;
            anim.SetBool("inRange", false);
            nav.isStopped = false;
            //nav.updateRotation = true;
        }
    }

    private void FixedUpdate()
    {
        if (nav.isStopped)
        {
            Vector3 vector = (player.position - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(vector);
            transform.rotation = newRotation;
        }
    }


    void RotateAndShoot()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("MageShootPoint");

        //Rotate
        Vector3 vector = (player.position - transform.position).normalized;
        Quaternion newRotation = Quaternion.LookRotation(vector);
        //transform.rotation = newRotation;


        //Vector3 playerToMouse = floorHit.point - transform.position;
        // Ensure the vector is entirely along the floor plane.
        //playerToMouse.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        //Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

        //player.GetComponent<PlayerMovement>().Shoot(newRotation);

        // instantiate arrow
        //GameObject newArrow = Instantiate(arrow, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
        //newArrow.transform.Rotate(0f, 90f, 5f);
        //newArrow.GetComponent<Rigidbody>().velocity = (floorHit.point - transform.position).normalized * arrowVelocity;

        timer = 0f;
        //gunAudio.Play();

        // instantiate magicball
        GameObject newMagicball = Instantiate(magicball, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
        newMagicball.transform.Rotate(0f, -90f, 90f);
        newMagicball.GetComponent<Rigidbody>().velocity = vector * magicballVelocity;
        audio.PlayOneShot(attackClip, 0.9f);
    }
}
