using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding {

    public static string BuildingTag = "Building";

    public BuildingSize Size;
    public Vector3 MenuPosition;
    public float MenuScale;

    Material originalMaterial;
    public Material HighlightedMaterial;
    Renderer highlightTarget;

    Cloudship player;

       bool highlight = false;

    void Awake()
    {
        highlightTarget = GetComponentInChildren<Renderer>();
        originalMaterial = highlightTarget.sharedMaterial;
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
             
        return clone.GetComponent<Building>();
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

}