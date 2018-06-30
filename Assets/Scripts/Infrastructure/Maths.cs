using UnityEngine;

public static class Maths
{
    public static float Rescale(float outLower, float outUpper, float inLower, float inUpper, float inValue)
    {
        if (inValue <= inLower)
        {
            return outLower;
        }
        else if (inValue >= inUpper)
        {
            return outUpper;
        }
        else
        {
            return ((outUpper - outLower) * ((inValue - inLower) / (inUpper - inLower))) + outLower;
        }
    }

    public static int RoundOffToNearestOrder(float value, int order)
    {
        var denominator = Mathf.Pow(10, order);
        var reduced = value / denominator;
        var rounded = Mathf.Round(reduced);
        return (int)(rounded * denominator);
    }

    public static Vector3 RoundOffToNearestOrder(Vector3 value, int order) => 
        new Vector3(
            RoundOffToNearestOrder(value.x, order),
            RoundOffToNearestOrder(value.y, order),
            RoundOffToNearestOrder(value.z, order));
}