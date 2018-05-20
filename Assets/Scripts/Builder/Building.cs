using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding {

    public static string BuildingTag = "Building";

    public Vector3 MenuPosition;
    public float MenuScale;
    public string Name;

    Renderer[] highlightTargets;
    public Shader standardShader;
    public Shader notYetPlacedShader;

    public bool InMenu = false;

    public bool AnotherObjectCollision;
    public bool BoundaryCollision;
    public bool IsOverCloudship;
    

    void Awake()
    {
        BoundaryCollision = true;
        AnotherObjectCollision = false;
        IsOverCloudship = false;

        highlightTargets = GetComponentsInChildren<Renderer>();
        standardShader = highlightTargets[0].materials[0].shader;
        notYetPlacedShader = Shader.Find("graphs/BuildingHighlight");
    }

    public void SetupForMenu(int menuLayer)
    {
        gameObject.layer = menuLayer;
        foreach(Transform child in transform)
        {
            child.gameObject.layer = menuLayer;    
        }
        transform.localRotation = Quaternion.identity;
        transform.localPosition = MenuPosition;
        transform.localScale = new Vector3(MenuScale,MenuScale,MenuScale);
        InMenu = true;
    }

    internal void RotateAntiClockwise()
    {
        var aimRotation = transform.localEulerAngles + new Vector3(0, -90, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRotation), Time.deltaTime);
    }

    internal void RotateClockwise()
    {
        var aimRotation = transform.localEulerAngles + new Vector3(0, 90, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRotation), Time.deltaTime);
    }

    public Building Clone(Transform buildSurface)
    {
        var clone = Instantiate(gameObject, Vector3.zero, Quaternion.identity, buildSurface);
        clone.tag = BuildingTag;
		clone.transform.localScale = Vector3.one;
        clone.transform.localRotation = Quaternion.identity;
        clone.transform.localPosition = Vector3.zero;
        clone.layer = 10;
        
        foreach(Transform child in clone.transform)
        {
            child.gameObject.layer = clone.layer;      
        }
        var cloneBuilding = clone.GetComponent<Building>();
        cloneBuilding.InMenu = false;
        return cloneBuilding;
    }

    public Bounds GetBounds()
    {
        var bounds = new Bounds (transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate (renderer.bounds);
        }
        return bounds;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void UpdateVisibility()
    {
        foreach(var target in highlightTargets)
        {
            foreach(var material in target.materials)
            {
                if (CanPlace)
                {
                    material.shader = standardShader;
                }
                else
                {
                    material.shader = notYetPlacedShader;
                }
            }
        }
    }

    public bool CanPlace
    {
        get
        {
            return BoundaryCollision && !AnotherObjectCollision && IsOverCloudship;
        }
    }
}