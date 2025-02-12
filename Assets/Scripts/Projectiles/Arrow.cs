using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public AudioClip hitClip;

    private AudioSource audio;

    public void Awake()
    {
        audio = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable") || other.gameObject.tag == "Sorceress" || other.gameObject.tag == "Wizard" || other.gameObject.tag == "Warrior")
        {
            audio.PlayOneShot(hitClip, 0.9f);
            Destroy(this.gameObject);
        }
    }
}
