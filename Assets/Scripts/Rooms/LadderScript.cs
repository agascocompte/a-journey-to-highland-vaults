using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

    public bool allowLadder;

    private float timer;    
    private bool playerOn;

    public AudioClip roomCompletedClip;

    private AudioSource audio;

    private void Awake()
    {
        playerOn = false;
        allowLadder = false;
        timer = 0;
        audio = GetComponent<AudioSource>();
    }

    public void Start()
    {
        audio.PlayOneShot(roomCompletedClip, 0.9f);

    }

    private void Update()
    {
        if (!playerOn)
        {
            timer += Time.deltaTime;
        }
        
        if (timer > 0.1f)
        {
            allowLadder = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerOn = false;
    }
}
