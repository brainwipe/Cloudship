using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollision : MonoBehaviour
{
    Building building;

    void Awake()
    {
        building = GetComponentInParent<Building>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter: Boundary: " + other.tag + ", " +other.gameObject.layer);
        if (building.InMenu)
        {
            return;
        }

        if (other.tag == "BuilderBoundary")
        {
            building.CanPlace = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit: Boundary: " + other.tag + ", " +other.gameObject.layer);
        if (building.InMenu)
        {
            return;
        }

        if (other.tag == "BuilderBoundary")
        {
            building.CanPlace = false;
        }
    }
}