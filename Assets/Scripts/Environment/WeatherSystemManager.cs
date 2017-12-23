using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour
{
    float perlinScale = 30f;

    public Vector3 GetCycloneForce(Vector3 cycloneForce)
    {
        return  WindDirectionAt(cycloneForce) * (Vector3.back * 5);
    }

    public Quaternion WindDirectionAt(Vector3 location)
    {
        float perlinX = location.x / perlinScale;
		float perlinZ = location.z / perlinScale;
		var noise = Mathf.PerlinNoise(perlinX,perlinZ);
        var noiseAngle = noise * 360;
        return Quaternion.AngleAxis(noiseAngle, Vector3.up);
    }
}