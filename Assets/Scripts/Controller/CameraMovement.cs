using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraMode {
        Game,
        Cinematic
    }
    
    public Transform CompassRose;

    public CameraMotion current;
    public CameraMotion game;
    public CameraMotion cinematic;
    
    public float MaxY = 80f;
    public float MinY = -30f;
    public float MinDistance = 61;
    public float MaxDistance = 230;

    Cloudship player;
    Vector3 offset = new Vector3(0, 67.7f, -146.2f);
    CameraMode mode;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
        Camera camera = GetComponentInChildren<Camera>();
        
        var proportionFromBottomLeftCorner = 0.16f;
        var screenPosition = new Vector3(proportionFromBottomLeftCorner, proportionFromBottomLeftCorner * camera.aspect, camera.nearClipPlane + 0.9f);
        ControllerOffset.position = camera.ViewportToWorldPoint(screenPosition);
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
                player.CinematicMode();
            }
            else
            {
                mode = CameraMode.Game;
                current = game;
                Cursor.visible = true;
                player.NormalMode();
            }
        }
    }

    void FixedUpdate()
    {
        var proposedOffset = offset;
        if (Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * current.Rotation, Vector3.up);
            proposedOffset = turnAngle * proposedOffset;
            proposedOffset = proposedOffset - new Vector3(0, Input.GetAxis("Mouse Y") * current.Vertical, 0);

            if(Input.GetAxis("Mouse ScrollWheel") < 0) // Back/Out
            {
                proposedOffset = proposedOffset + (10f * offset.normalized);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0) // Forward/In
            {
                proposedOffset = proposedOffset - (10f * offset.normalized);
            }

            var proposedMagnitude = proposedOffset.magnitude;
            if (proposedMagnitude < MaxDistance && proposedMagnitude > MinDistance)
            {
                offset = proposedOffset;
            }
        }



        Vector3 targetPosition = player.transform.position + offset;
        float clampY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
        targetPosition = new Vector3(targetPosition.x, clampY, targetPosition.z);
        transform.position = Vector3.Slerp(transform.position, targetPosition, current.Smooth);

        transform.LookAt(player.transform.position);

        CompassRose.localRotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    } 

    Transform ControllerOffset => GetComponentInChildren<Controller>().transform.parent;

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