using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedArrow : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("SpeedArrowImage").GetComponent<Image>().enabled = true;

            ScoreManager.score += 10;
            other.gameObject.GetComponentInChildren<PlayerShooting>().ApplyMoreSpeed(0.45f);
            Destroy(this.gameObject);
        }
    }
}
