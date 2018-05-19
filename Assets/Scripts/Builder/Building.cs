using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding {

    public static string BuildingTag = "Building";

    public Vector3 MenuPosition;
    public float MenuScale;
    public string Name;

    Renderer highlightTarget;
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

        highlightTarget = GetComponentInChildren<Renderer>();
        standardShader = highlightTarget.materials[0].shader;
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


    public void Remove()
    {
        Destroy(gameObject);
    }

    public void UpdateVisibility()
    {
        foreach(var material in highlightTarget.materials)
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

    public bool CanPlace
    {
        get
        {
            return BoundaryCollision && !AnotherObjectCollision && IsOverCloudship;
        }
    }
}