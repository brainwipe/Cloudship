using System;
using UnityEngine;
public static class UnityExtensions
{
    public static float[] ToArray(this Vector3 vector) => new[] { vector.x, vector.y, vector.z };

    public static void FromArray(this Vector3 vector, float[] array)
    {
        vector.x = array[0];
        vector.y = array[1];
        vector.z = array[2];
    }

    public static Vector3 ToVector(this float[] array) => new Vector3(array[0], array[1], array[2]);
 
    public static float[] ToArray(this Quaternion quaternion) => quaternion.eulerAngles.ToArray();
    
    public static Quaternion ToQuaternion(this float[] array) => Quaternion.Euler(array[0], array[1], array[2]);
}