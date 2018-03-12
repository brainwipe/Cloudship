using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    private Vector3 offset;

    [Range(0.01f, 0.4f)]
    public float SmoothFactor = 0.2f;

    public bool LookAtPlayer = true;

    public bool RotateAroundPlayer = true;

    public float RotationSpeed = 5f;

    void Start()
    {
        offset = transform.position - Player.position;
    }

    void FixedUpdate()
    {
        if (RotateAroundPlayer && Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            offset = turnAngle * offset;
        }

        Vector3 targetPosition = Player.position + offset;
        var newPos = Vector3.Slerp(transform.position, targetPosition, SmoothFactor);
        transform.position = newPos;
        
        if (LookAtPlayer || RotateAroundPlayer)
        {
           transform.LookAt(Player);
        }
    } 
}