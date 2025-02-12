using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceressAttack : MonoBehaviour
{
    private Transform player;
    private Animator anim;
    private Rigidbody sorceressRigidBody;

    public GameObject fireball;
    public float range = 5f;
    public float fireballVelocity = 10f;
    public float timeBetweenFireballs = 2f;

    float startTime = 0.8f;
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
        sorceressRigidBody = GetComponent<Rigidbody>();
        timer = startTime;
    }

    void Update()
    {
        float actual_distance = Vector3.Distance(transform.position, player.position);

        if (actual_distance < range)
        {
            anim.SetBool("inRange", true);
            timer += Time.deltaTime;

            if (timer >= timeBetweenFireballs) //&& Time.timeScale != 0)
            {
                RotateAndShoot();
            }
        }
        else
        {
            timer = startTime;
            anim.SetBool("inRange", false);
        }
    }

    void RotateAndShoot()
    {
        //Rotate
        Vector3 vector = (player.position - transform.position).normalized;
        Quaternion newRotation = Quaternion.LookRotation(vector);
        sorceressRigidBody.MoveRotation(newRotation);

        timer = 0f;
        //gunAudio.Play();

        // instantiate fireball
        GameObject newFireBall = Instantiate(fireball, new Vector3(transform.position.x, 1.7f, transform.position.z), newRotation);
        newFireBall.transform.Rotate(0f, -90f, 90f);
        newFireBall.GetComponent<Rigidbody>().velocity = vector * fireballVelocity;

        audio.PlayOneShot(attackClip, 0.9f);
    }
}
