using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraMode {
        Game,
        Cinematic
    }
    
    public Transform Controller;
    public Transform CompassRose;

    public CameraMotion current;
    public CameraMotion game;
    public CameraMotion cinematic;
    
    public float MaxY = 80f;
    public float MinY = -20f;

    Cloudship player;
    Vector3 offset;
    CameraMode mode;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
        Camera camera = GetComponentInChildren<Camera>();
        offset = transform.position - player.transform.position;
        var v3Pos = new Vector3(0.12f, 0.25f, 1.5f);
        Controller.transform.position = camera.ViewportToWorldPoint(v3Pos);
        current = game;

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (mode == CameraMode.Game)
            {
                mode = CameraMode.Cinematic;
                current = cinematic;
                Cursor.visible = false;
            }
            else
            {
                mode = CameraMode.Game;
                current = game;
                Cursor.visible = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * current.Rotation, Vector3.up);
            offset = turnAngle * offset;

            offset = offset - new Vector3(0, Input.GetAxis("Mouse Y") * current.Vertical, 0);
        }

        Vector3 targetPosition = player.transform.position + offset;
        float clampY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
        targetPosition = new Vector3(targetPosition.x, clampY, targetPosition.z);
        transform.position = Vector3.Slerp(transform.position, targetPosition, current.Smooth);

        transform.LookAt(player.transform.position);

        CompassRose.localRotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    } 

    [System.Serializable]    
    public class CameraMotion
    {
        public CameraMotion(float rotation, float vertical, float smooth)
        {
            Rotation = rotation;
            Vertical = vertical;
            Smooth = smooth;
        }
        
        public float Rotation;
        public float Vertical;
        [Range(0.01f, 0.8f)]
        public float Smooth; 

    }
}