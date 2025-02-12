using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public GameObject audioOn;
    public GameObject audioOff;
    public static bool isVolumeActive = true;
    
    public void enableSound()
    {
        audioOff.SetActive(false);
        audioOn.SetActive(true);
        isVolumeActive = true;
    }

    public void disableSound()
    {
        audioOn.SetActive(false);
        audioOff.SetActive(true);
        isVolumeActive = false;
    }
}
