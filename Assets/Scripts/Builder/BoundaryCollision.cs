using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollision : MonoBehaviour
{
    public Building building;

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

        if (other.tag == "BuilderBoundary")
        {
            building.BoundaryCollision = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (building.InMenu)
        {
            return;
        }

        if (other.tag == "BuilderBoundary")
        {
            building.BoundaryCollision = false;
        }
    }
}