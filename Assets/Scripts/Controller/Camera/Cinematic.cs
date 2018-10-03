using UnityEngine;

[System.Serializable]
public class Cinematic : ThirdPerson
{
    public Cinematic(Transform cameraTransform) : base (cameraTransform)
    {
        Rotation = 9f;
        Vertical = 6f;
        Smooth = 0.1f;
    }
}