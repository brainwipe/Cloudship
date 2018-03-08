using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    private Vector3 offset;

    [Range(0.01f, 0.4f)]
    public float SmoothFactor = 0.2f;

    public bool LookAtPlayer = false;

    public Vector3 newPos;

    void Start()
    {
        offset = transform.position - Player.position;
    }

    void FixedUpdate()
    {
       Vector3 targetPosition = Player.position + offset;
       newPos = Vector3.Slerp(transform.position, targetPosition, SmoothFactor);
       transform.position = newPos;
        if (LookAtPlayer)
        {
           transform.LookAt(Player);
        }
    } 
}