using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraMode
    {
        Game,
        Cinematic
    }

    public CameraMotion current;
    public CameraMotion game;
    public CameraMotion cinematic;
    Cloudship player;
    Vector3 offset = new Vector3(0, 67.7f, -146.2f);
    float zoomDistance = 0f;
    float minZoom = 55f;
    float maxZoom = 300f;

    float proportionFromBottomLeftCorner = 0.16f;
    CameraMode mode;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
        Camera camera = GetComponentInChildren<Camera>();

        var screenPosition = new Vector3(proportionFromBottomLeftCorner, proportionFromBottomLeftCorner * camera.aspect, camera.nearClipPlane + 0.9f);
        ControllerOffset.position = camera.ViewportToWorldPoint(screenPosition);
        zoomDistance = (transform.position - player.Position).magnitude;
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
        transform.position = player.transform.position + offset;
        var savedPosition = transform.position;

        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * current.Rotation;
            transform.RotateAround(player.transform.position, new Vector3(0,1,0), horizontal);

            float vertical = Input.GetAxis("Mouse Y") * current.Vertical * -1;
            transform.RotateAround(player.transform.position, transform.right, vertical);

            if (transform.rotation.eulerAngles.x > 79f && transform.rotation.eulerAngles.x < 300)
            {
                transform.position = savedPosition;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var currentZoom = (transform.position - player.transform.position).magnitude;
            var zoomStep = 110f;
            if (currentZoom < 140)
            {
                zoomStep = 80f;
            }
            if (currentZoom < 120)
            {
                zoomStep = 50f;
            }
            if (currentZoom < 60)
            {
                zoomStep = 20f;
            }
            zoomDistance += -Input.GetAxis("Mouse ScrollWheel") * zoomStep;
            zoomDistance = Mathf.Clamp(zoomDistance, minZoom, maxZoom);
        }

        transform.position = player.transform.position + ((transform.position - player.transform.position).normalized * zoomDistance);

        transform.position = Vector3.Slerp(savedPosition, transform.position, current.Smooth);
        offset = transform.position - player.transform.position;
        transform.LookAt(player.transform.position);
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
        [Range(0.001f, 0.7f)]
        public float Smooth;

    }
}