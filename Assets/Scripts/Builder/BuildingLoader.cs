using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class BuildingLoader 
{
    private static string BuildingResourceLocation => "Buildings";
    public static GameObject[] Load()
    {
        return Resources.LoadAll<GameObject>(BuildingResourceLocation);
    }

    public static GameObject Load(string name)
    {
        var path = Path.Combine(BuildingResourceLocation, name);
        return Resources.Load<GameObject>(path + ".prefab");
        
    }
}