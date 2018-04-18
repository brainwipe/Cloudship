using UnityEngine;

public class Grid
{
    private float GridSize = 10f;

    public int X { get; set; }
    public int Y { get; set; }

    public Vector3 ToWorld()
    {
        return new Vector3(X * GridSize, 0, Y * GridSize);
    }

    public static Grid From(int x, int y){
        return new Grid {
            X = x,
            Y = y
        };
    }
}