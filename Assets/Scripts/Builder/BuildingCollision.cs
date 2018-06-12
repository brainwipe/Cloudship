using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    Building building;

    void Awake()
    {
        building = GetComponentInParent<Building>();
    }

    void OnTriggerStay(Collider other)
    {
        if (building.InMenu)
        {
            return;
        }

        if (other.tag == "Building")
        {
            building.AnotherObjectCollision = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (building.InMenu)
        {
            return;
        }
        
        if (other.tag == "Building")
        {
            building.AnotherObjectCollision = false;
        }
    }
}
