using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    [Range(0,1)] public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private void Start()
    {
        AudioListener.volume = 1;
    }
    private void LateUpdate()
    {
        Vector3 camPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, camPos, smoothSpeed);
        transform.position = smoothedPos;
        transform.LookAt(target);
    }
}
