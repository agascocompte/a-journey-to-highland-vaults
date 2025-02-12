using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform playerTransform;
    [Range(0.01f, 1.0f)]
    public float smoothFactor = 1f;

    private Vector3 cameraOffset;

    private void Start()
    {
        cameraOffset = transform.position - playerTransform.position;
    }
    void Update()
    {
        Vector3 goalPos = playerTransform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, goalPos, smoothFactor);

        Vector3 behindPlayer = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z - 3);
        transform.LookAt(behindPlayer);

    }
}
