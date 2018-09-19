using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public ICameraMode[] Modes;

    Cloudship player;

    float proportionFromBottomLeftCorner = 0.16f;
    
    int currentMode = 0;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
        Camera camera = GetComponentInChildren<Camera>();

        var screenPosition = new Vector3(proportionFromBottomLeftCorner, proportionFromBottomLeftCorner * camera.aspect, camera.nearClipPlane + 0.9f);
        ControllerOffset.position = camera.ViewportToWorldPoint(screenPosition);

        Modes = new[] {
            new ThirdPerson(transform, player),
            new Cinematic(transform, player),
        };
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            currentMode++;
            if (currentMode > Modes.Length - 1)
            {
                currentMode = 0;
            }
            
            Modes[currentMode].Selected();
        }
    }

    void FixedUpdate()
    {
        Modes[currentMode].FixedUpdate();
    }

    Transform ControllerOffset => GetComponentInChildren<Controller>().transform.parent;
}