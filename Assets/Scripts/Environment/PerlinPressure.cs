using UnityEngine;

public class PerlinPressure : IWindMaker
{
    float perlinScale = 2000f;
    float perlinOffset = 20000;
    float gradientSampleWidth = 20f;
    float windStrength = 10000000f;

    public Vector3 GetCycloneForce(Vector3 cycloneForce)
    {
        if (!GameManager.Instance.Features.WindMovement)
        {
            return Vector3.zero;
        }
        var vectorGradient = FindVectorGradient(cycloneForce);
        return  Direction(vectorGradient) * (Vector3.back * windStrength * vectorGradient.magnitude);
    }

    private Quaternion Direction(Vector3 vectorGradient)
    {
        var  fromHighToLow = Quaternion.FromToRotation(Vector3.forward,  vectorGradient);
        return fromHighToLow *= Quaternion.Euler(0, 90, 0);
    }

    private float ScalePerlin(float location)
    {
        return (location /  perlinScale) + perlinOffset;
    }
    
    private Vector3 FindVectorGradient(Vector3 location)
    {
        var lowerX = Mathf.PerlinNoise(ScalePerlin(location.x - gradientSampleWidth), ScalePerlin(location.z));
        var upperX = Mathf.PerlinNoise(ScalePerlin(location.x + gradientSampleWidth), ScalePerlin(location.z));
        var lowerZ = Mathf.PerlinNoise(ScalePerlin(location.x), ScalePerlin(location.z - gradientSampleWidth));
        var upperZ = Mathf.PerlinNoise(ScalePerlin(location.x), ScalePerlin(location.z + gradientSampleWidth));

        return new Vector3(upperX - lowerX, 0, upperZ - lowerZ);
    }
}