using UnityEngine;

public class PerlinPressure : MonoBehaviour, IWindMaker
{
    float perlinScale = 22f;
    float perlinOffset = 20000;
    float gradientSampleWidth = 0.5f;
    float windStrength = 5f;

    public Vector3 GetCycloneForce(Vector3 cycloneForce)
    {
        return  WindDirectionAt(cycloneForce) * (Vector3.back * windStrength * WindMagnitudeAt(cycloneForce));
    }

    public Quaternion WindDirectionAt(Vector3 location)
    {
        var fromHighToLow = FindGradient(location);

        return fromHighToLow *= Quaternion.Euler(0, 90, 0);
    }

    public float WindMagnitudeAt(Vector3 location)
    {
        return 1 - Mathf.PerlinNoise(ScalePerlin(location.x), ScalePerlin(location.z));
    }

    private float ScalePerlin(float location)
    {
        return (location /  perlinScale) + perlinOffset;
    }

    private Quaternion FindGradient(Vector3 location)
    {
        var lowerX = Mathf.PerlinNoise(ScalePerlin(location.x - gradientSampleWidth), ScalePerlin(location.z));
        var upperX = Mathf.PerlinNoise(ScalePerlin(location.x + gradientSampleWidth), ScalePerlin(location.z));
        var lowerZ = Mathf.PerlinNoise(ScalePerlin(location.x), ScalePerlin(location.z - gradientSampleWidth));
        var upperZ = Mathf.PerlinNoise(ScalePerlin(location.x), ScalePerlin(location.z + gradientSampleWidth));

        var vec = new Vector3(upperX - lowerX, 0, upperZ - lowerZ);
        return Quaternion.FromToRotation(Vector3.forward, vec);
    }
}