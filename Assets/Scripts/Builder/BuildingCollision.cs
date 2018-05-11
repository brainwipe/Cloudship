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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter: Collision: " + other.tag + ", " +other.gameObject.layer);
        if (building.InMenu)
        {
            return;
        }

        if (other.tag != "BuilderBoundary")
        {
            building.CanPlace = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit: Collision: " + other.tag + ", " +other.gameObject.layer);
        if (building.InMenu)
        {
            return;
        }
        
        if (other.tag != "BuilderBoundary")
        {
            building.CanPlace = true;
        }
    }
}
