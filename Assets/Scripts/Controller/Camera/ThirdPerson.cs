using UnityEngine;

[System.Serializable]
public class ThirdPerson : ICameraMode
{
    protected float Rotation = 9;
    protected float Vertical = 6;
    protected float Smooth = 0.6f;
    float zoomDistance = 0f;
    float minZoom = 55f;
    float maxZoom = 300f;
    Vector3 offset = new Vector3(0, 67.7f, -146.2f);

    Cloudship player;
    Transform cameraTransform;

    public ThirdPerson(Transform cameraTransform, Cloudship player)
    {
        this.cameraTransform = cameraTransform;
        this.player = player;
        zoomDistance = (cameraTransform.position - player.Position).magnitude;
    }

    public void Selected()
    {
        Cursor.visible = true;
        player.NormalMode();
        cameraTransform.parent = null;
    }

    public void FixedUpdate()
    {
        var desired = player.transform.position + offset;
        var savedPosition = cameraTransform.position;

        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * Rotation;
            cameraTransform.RotateAround(player.transform.position, new Vector3(0,1,0), horizontal);

            float vertical = Input.GetAxis("Mouse Y") * Vertical * -1;
            cameraTransform.RotateAround(player.transform.position, cameraTransform.right, vertical);

            if (cameraTransform.rotation.eulerAngles.x > 79f && cameraTransform.rotation.eulerAngles.x < 300)
            {
                cameraTransform.position = savedPosition;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var currentZoom = (cameraTransform.position - player.transform.position).magnitude;
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

        cameraTransform.position = player.transform.position + ((cameraTransform.position - player.transform.position).normalized * zoomDistance);

        cameraTransform.position = Vector3.Slerp(savedPosition, cameraTransform.position, Smooth);
        offset = cameraTransform.position - player.transform.position;
        cameraTransform.LookAt(player.transform.position);
    }

    
}