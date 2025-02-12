using UnityEngine;

public class HideWall : MonoBehaviour
{    void Start()
    {
        foreach (var gb in GetComponentsInChildren<MeshRenderer>())
        {
            gb.enabled = false;
        }
    }
}

