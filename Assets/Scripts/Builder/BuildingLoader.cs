using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public static class BuildingLoader 
{
    private static string BuildingResourceLocation => "Buildings";

    static GameObject[] buildingPrefabs;

    static GameObject[] BuildingPrefabs
    {
        get
        {
            if (buildingPrefabs == null || buildingPrefabs.Length == 0)
            {
                buildingPrefabs = Resources.LoadAll<GameObject>(BuildingResourceLocation);
            }
            return buildingPrefabs;
        }
    }

    public static GameObject[] Load()
    {
        return BuildingPrefabs;
    }

    public static GameObject Find(string id) 
    {
        foreach(var prefab in BuildingPrefabs)
        {
            var building = prefab.GetComponent<Building>();
            if (id == building.Id)
            {
                return prefab;
            }
        }
        throw new InvalidOperationException($"Building of id {id} was not found");
    }
        
    
}