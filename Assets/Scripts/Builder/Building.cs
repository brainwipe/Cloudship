using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, IHaveGridSpace {

    public Grid GridSpaceLocation { get; set; }
    public BuildingSize Size;
    
    public void SetupForMenu(int menuLayer)
    {
        gameObject.layer = menuLayer;
        transform.localRotation = Quaternion.identity;

        var bounds = CalculateUnscaledBounds();

        float newScale = (1 / bounds.extents.magnitude) * 0.11f;
        transform.localScale = new Vector3(newScale * 2, newScale * 2, newScale * 2);

        var offsetPosition = new Vector3(0, bounds.center.y * newScale, 0);
        transform.localPosition -= offsetPosition;
    }

    private Bounds CalculateUnscaledBounds()
    {
        var bounds = new Bounds(transform.position, Vector3.zero);
 
        foreach(MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            bounds.Encapsulate(meshFilter.mesh.bounds);
        }
        return bounds;
    }

}