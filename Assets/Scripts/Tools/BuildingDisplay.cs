using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisplay : MonoBehaviour {

    public bool RunOnLoad = false;


    void Start()
    {
        if (!RunOnLoad)
        {
            return;
        }

        var buildingPrefabs = BuildingLoader.Load();

        for(var i = 0; i < buildingPrefabs.Length; i++)
        {
            var prefab = buildingPrefabs[i];
            var building = Instantiate(prefab, Vector3.zero, Quaternion.identity, this.transform);
            var collider = building.GetComponent<Collider>();
            building.transform.position = (new Vector3(i * 20f, collider.bounds.size.y/2, 0)) + this.transform.position;
        }
    }
}