using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, IHaveGridSpace {

    public Grid GridSpaceLocation { get; set; }
    public BuildingSize Size;
    public Vector3 MenuPosition;
    public float MenuScale;
    
    public void SetupForMenu(int menuLayer)
    {
        gameObject.layer = menuLayer;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = MenuPosition;
        transform.localScale = new Vector3(MenuScale,MenuScale,MenuScale);
    }
}