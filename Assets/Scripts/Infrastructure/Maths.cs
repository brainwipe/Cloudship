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
            return (outUpper - outLower) * ((inValue - inLower) / (inUpper - inLower)) + outLower;
        }
    }
}