using UnityEngine;

public class WeatherSystemManager : MonoBehaviour
{
    float perlinScale = 22f;
    int samplesForGradient = 4;
    float sampleMultiplier = 2f;

    float windStrength = 5f;

    public Vector3 GetCycloneForce(Vector3 cycloneForce)
    {
        return  WindDirectionAt(cycloneForce) * (Vector3.back * windStrength * WindMagnitudeAt(cycloneForce));
    }

    public float WindMagnitudeAt(Vector3 location)
    {
        float sumOfNoiseDifference = 0;
        float noiseAtlocation = PerlinAt(location);


        for(int i=0; i<samplesForGradient; i++)
        {
            var purturbation = new Vector3(
                (Random.value - 0.5f) * sampleMultiplier, 
                0, 
                (Random.value - 0.5f) * sampleMultiplier);
            sumOfNoiseDifference +=  PerlinAt(location + purturbation) - noiseAtlocation;
        }

        return NormaliseMagnitude(Mathf.Abs(sumOfNoiseDifference / samplesForGradient));
    }

    public Quaternion WindDirectionAt(Vector3 location)
    {
		var noise = PerlinAt(location);
        var noiseAngle = noise * 360;
        return Quaternion.AngleAxis(noiseAngle, Vector3.up);
    }

    private float NormaliseMagnitude(float rawMagnitude)
    {
        return (rawMagnitude + 0.01f) / 0.035f;
    }

    private float PerlinAt(Vector3 location)
    {
        float perlinX = location.x / perlinScale;
		float perlinZ = location.z / perlinScale;
		return Mathf.PerlinNoise(perlinX,perlinZ);
    }
}