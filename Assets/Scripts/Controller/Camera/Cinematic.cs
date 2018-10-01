using UnityEngine;

[System.Serializable]
public class Cinematic : ThirdPerson
{
    public Cinematic(Transform cameraTransform, Cloudship player) : base (cameraTransform, player)
    {
        Rotation = 9f;
        Vertical = 6f;
        Smooth = 0.1f;
    }
}