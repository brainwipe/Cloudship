using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public static float GridSize = 10f;
    public static Vector3 Offset = new Vector3(GridSize / 2, 0, GridSize / 2);

    public int X { get; set; }
    public int Z { get; set; }

    public Vector3 ToWorld()
    {
        return new Vector3(X * GridSize, 0, Z * GridSize) + Offset;
    }

    public static Grid From(int x, int z){
        return new Grid {
            X = x,
            Z = z
        };
    }

    public IEnumerable<Grid> GetAllLocations(BuildingSize size)
    {
        for(int width = 0; width < size.WidthX; width++)
        {
            for(int length =0; length < size.LengthZ; length++)	
            {
                yield return new Grid{
                    X = X + width,
                    Z = Z + length
                };
            }
        }
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