using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingLoader {
    public static GameObject[] Load()
    {
        return Resources.LoadAll<GameObject>("BuildingPrefabs");
    }
}