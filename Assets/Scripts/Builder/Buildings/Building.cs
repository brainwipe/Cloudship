using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, ITakeDamage, IHaveAbilities {

    public static string BuildingTag = "Building";
    public string Id;
    public IAmAShip owner;
    Renderer[] highlightTargets;
    Rigidbody rigidBody;

    public Vector3 MenuPosition;
    public float MenuScale;
    [HideInInspector]
    public bool InMenu = false;
    //[HideInInspector]
    public bool AnotherObjectCollision;
    //[HideInInspector]
    public bool BoundaryCollision;
    //[HideInInspector]
    public bool IsOverCloudship;
    //[HideInInspector]
    public Bounds PreCalculatedBounds;

    public float Health;
    public float FlotsamCost;
    public Abilities Abilities;

    void Awake()
    {
        BoundaryCollision = false;
        AnotherObjectCollision = false;
        IsOverCloudship = false;

        highlightTargets = GetComponentsInChildren<Renderer>();
        owner = GetComponentInParent<IAmAShip>();
        rigidBody = GetComponent<Rigidbody>();
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
        var clone = Instantiate(gameObject, transform.position, Quaternion.identity);
        var building = clone.GetComponent<Building>();
        building.Reset(buildSurface);
        return building;
    }

    public void Reset(Transform buildSurface)
    {
        gameObject.SetActive(false);
        gameObject.layer = 10;
        foreach(Transform child in transform)
        {
            child.gameObject.layer = 10;      
        }
        
        tag = BuildingTag;
        transform.localScale = Vector3.one;

        CalculateBounds();
        
        transform.SetParent(buildSurface, true);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        InMenu = false;
        Selected();
        gameObject.SetActive(true);
    }

    public void CalculateBounds()
    {
        PreCalculatedBounds = new Bounds (transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderers)
        {
            PreCalculatedBounds.Encapsulate (renderer.bounds);
        }
    }

    public void Remove() => Destroy(gameObject);

    void OnDestroy()
    {
        if (!InMenu)
        {
            if (owner != null)
            {
                owner.UpdateAbilities();
            }
        }
    }

    public Vector3 Position 
    {
        get 
        {
            return transform.position;
        }
        set
        {
            rigidBody.MovePosition(value);
        }
    }

    public Quaternion Rotation
    {
        get 
        {
            return transform.rotation;
        }
        set 
        {
            rigidBody.MoveRotation(value);
        }
    }

    public void Selected()
    {
        SetShaderBool("Boolean_1662BEBB", true);
    }

    public void UnSelected()
    {
        SetShaderBool("Boolean_1662BEBB", false);
    }

    public void UpdateAffordability(float playerTotalFlotsam)
    {
        if (CanAfford(playerTotalFlotsam))
        {
            SetShaderBool("Boolean_A5D81C01", false);
        }
        else
        {
            SetShaderBool("Boolean_A5D81C01", true);
        }
    }

    public void UpdateVisibility()
    {
        if (CanPlace)
        {
            SetShaderBool("Boolean_A5D81C01", false);
        }
        else
        {
            SetShaderBool("Boolean_A5D81C01", true);
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
        if (!GameManager.Instance.Mode.PlayerTakesDamage && owner.IAmAPlayer)
        {
            return;
        }

        Health -= amount;
        if (Health < 1)
        {
            Remove();
        }
    }

    public bool CanPlace => BoundaryCollision && !AnotherObjectCollision && IsOverCloudship;
    
    public bool CanAfford(float playerTotalFlotsam) => playerTotalFlotsam >= FlotsamCost;

    public Abilities Skills => Abilities;
}