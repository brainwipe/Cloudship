using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, ITakeDamage, IHaveAbilities {

    public static string BuildingTag = "Building";

    public Cloudship player;

    public Vector3 MenuPosition;
    public float MenuScale;
    public string Name;

    Renderer[] highlightTargets;
    public bool InMenu = false;

    public bool AnotherObjectCollision;
    public bool BoundaryCollision;
    public bool IsOverCloudship;

    public float Health;
    public Abilities Abilities;
    

    void Awake()
    {
        BoundaryCollision = true;
        AnotherObjectCollision = false;
        IsOverCloudship = false;

        highlightTargets = GetComponentsInChildren<Renderer>();
        player = GetComponentInParent<Cloudship>();
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
        cloneBuilding.Selected();
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

    public void Remove() => Destroy(gameObject);

    void OnDestroy()
    {
        if (!InMenu)
        {
            player.UpdateAbilities();   
        }
    }

    public void Hover() => SetShaderBool("Boolean_8EBFB74C", true);
    
    public void UnHover() => SetShaderBool("Boolean_8EBFB74C", false);
    
    public void Selected()
    {
        SetShaderBool("Boolean_ED362197", true);
    }

    public void UnSelected()
    {
        SetShaderBool("Boolean_ED362197", false);
    }

    public void UpdateVisibility()
    {
        if (CanPlace)
        {
            SetShaderBool("Boolean_8F81C679", false);
        }
        else
        {
            SetShaderBool("Boolean_8F81C679", true);
        }
    }

    void SetShaderBool(string shaderPropertyId, bool value)
    {
        float shaderValue = value ? 1 : 0;

        foreach(var target in highlightTargets)
        {
            if (target == null)
            {
                continue;
            }
            foreach(var material in target.materials)
            {
                material.SetFloat(shaderPropertyId, shaderValue);
            }
        }
    }

    public void Damage(float amount)
    {
        Health -= amount;
        if (Health < 1)
        {
            Remove();
        }
    }


    public bool CanPlace => BoundaryCollision && !AnotherObjectCollision && IsOverCloudship;
    
    public Abilities Skills => Abilities;
}