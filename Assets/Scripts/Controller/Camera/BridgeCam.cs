using UnityEngine;

public class BridgeCam : ICameraMode
{
    Transform cameraTransform;
    float Smooth = 0.6f;
    float Rotation = 9f;

    float Vertical = 6f;
    public BridgeCam(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    public void FixedUpdate()
    {
        var bridge = GameManager.Instance.PlayerCloudship.GetComponentInChildren<Bridge>();
        
        cameraTransform.localPosition = bridge.CamPosition;
        var savedPosition = cameraTransform.position;

        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * Rotation;
            cameraTransform.Rotate(Vector3.up, horizontal, Space.World);

            float vertical = Input.GetAxis("Mouse Y") * Vertical * -1;
            cameraTransform.Rotate(cameraTransform.right, vertical, Space.World);
        }

        cameraTransform.position = Vector3.Slerp(savedPosition, cameraTransform.position, Smooth);
    }

    public void Selected()
    {
        var bridge = GameManager.Instance.PlayerCloudship.GetComponentInChildren<Bridge>();
        cameraTransform.parent = bridge.gameObject.transform;
    }
}