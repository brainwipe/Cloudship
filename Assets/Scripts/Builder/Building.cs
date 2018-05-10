using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding {

    public static string BuildingTag = "Building";

    public BuildingSize Size;
    public Vector3 MenuPosition;
    public float MenuScale;
    public string Name;

    Material originalMaterial;
    public Material HighlightedMaterial;
    Renderer highlightTarget;

    Cloudship player;

    bool highlight = false;
    public bool InMenu = false;

    public bool CanPlace { get; private set; }

    void Awake()
    {
        highlightTarget = GetComponentInChildren<Renderer>();
        originalMaterial = highlightTarget.sharedMaterial;
        CanPlace = true;
    }

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
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
        clone.layer = 10;
        
        gameObject.layer = player.gameObject.layer;
        foreach(Transform child in transform)
        {
            child.gameObject.layer = player.gameObject.layer;      
        }
        var cloneBuilding = clone.GetComponent<Building>();
        cloneBuilding.InMenu = false;
        return cloneBuilding;
    }

    public void ToggleHighlight()
    {
        if (highlight)
        {
            highlightTarget.sharedMaterial = originalMaterial;
        }
        else
        {
            highlightTarget.sharedMaterial = HighlightedMaterial;
        }
        highlight = !highlight;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void UpdateVisibility()
    {
        foreach(var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled =CanPlace;
        }
		
    }

    void OnTriggerEnter(Collider other)
    {
        if (InMenu)
        {
            return;
        }

        if (other.tag == "BuilderBoundary")
        {
            CanPlace = true;
            return;
        }
        CanPlace = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (InMenu)
        {
            return;
        }

        if (other.tag == "BuilderBoundary")
        {
            CanPlace = false;
            return;
        }
        CanPlace = true;
    }

}