using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleArrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("TripleArrowImage").GetComponent<Image>().enabled = true;

            ScoreManager.score += 20;
            other.gameObject.GetComponentInChildren<PlayerShooting>().ApplyTripleShot();
            Destroy(this.gameObject);
        }
    }
}
