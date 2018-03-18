using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    public Transform Controller;
    public Transform CompassRose;
    private Vector3 offset;

    [Range(0.01f, 0.4f)]
    public float SmoothFactor = 0.6f;

    public float RotationSpeed = 5f;

    public float MaxY = 1.15f;
    public float MinY = -0.75f;


    void Start()
    {
        Camera camera = GetComponentInChildren<Camera>();
        offset = transform.position - Player.position;
        var v3Pos = new Vector3(0.15f, 0.25f, 0.8f);
        Controller.transform.position = camera.ViewportToWorldPoint(v3Pos);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            offset = turnAngle * offset;

            offset = offset - new Vector3(0, Input.GetAxis("Mouse Y") * 0.05f, 0);
        }

        Vector3 targetPosition = Player.position + offset;

        float clampY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
      
        targetPosition = new Vector3(targetPosition.x, clampY, targetPosition.z);

        transform.position = Vector3.Slerp(transform.position, targetPosition, SmoothFactor);
        transform.LookAt(Player);

        CompassRose.localRotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    } 
}