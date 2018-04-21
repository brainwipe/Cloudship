using System;

[Serializable]
public class BuildingSize 
{
    public int WidthX = 1; 
    public int LengthZ = 1;

    public bool IsLargerThanUnit 
    {
        get{
            return WidthX > 1 || LengthZ > 1;
        }
    }
}