using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorMovement : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;

    private Animator anim;

    public float range = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(player.position);

        Position();
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

    void Position()
    {
        float actual_distance = Vector3.Distance(transform.position, player.position);

        if (actual_distance <= range)
        {
            anim.SetBool("inRange", true);
            nav.isStopped = true;
        }
        else
        {
            anim.SetBool("inRange", false);
            nav.isStopped = false;
        }
    }
}
