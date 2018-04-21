using UnityEngine;

public class Grid
{
    private float GridSize = 10f;

    public int X { get; set; }
    public int Z { get; set; }

    public Vector3 ToWorld()
    {
        return new Vector3(X * GridSize, 0, Z * GridSize);
    }

    public static Grid From(int x, int z){
        return new Grid {
            X = x,
            Z = z
        };
    }

    public override bool Equals(object obj) 
    {
        if (obj == null || GetType() != obj.GetType()) 
        {
            return false;
        }

        Grid p = (Grid)obj;
        return (X == p.X) && (Z == p.Z);
    }

    public override int GetHashCode() 
    {
        return X ^ Z;
    }
}