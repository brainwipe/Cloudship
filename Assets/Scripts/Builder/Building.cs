using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, IHaveGridSpace {

    public static string BuildingTag = "Building";

    public BuildingSize Size;
    public Vector3 MenuPosition;
    public float MenuScale;

    Grid gridSpaceLocation;
    public Grid GridSpaceLocation 
    { 
        get{
            return gridSpaceLocation;
        }
        set{
            gridSpaceLocation = value;
            transform.localPosition = LocalLocation;
        }
    }

    public void SetupForMenu(int menuLayer)
    {
        gameObject.layer = menuLayer;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = MenuPosition;
        transform.localScale = new Vector3(MenuScale,MenuScale,MenuScale);
    }

    public IEnumerable<Grid> Locations
    {
        get 
        {
            return gridSpaceLocation.GetAllLocations(Size);
        }
    }

    public Building Clone(Transform buildSurface)
    {
        var clone = Instantiate(gameObject, Vector3.zero, Quaternion.identity, buildSurface);
        clone.tag = BuildingTag;
		clone.transform.localScale = Vector3.one;
        clone.transform.localRotation = Quaternion.identity;

        return clone.GetComponent<Building>();
    }

    private Vector3 LocalLocation
    {
        get {
            var buildingSizeOffset = Vector3.zero;
            if (Size.IsLargerThanUnit)
            {
                buildingSizeOffset = new Vector3(
                    ((Size.WidthX) / 2) * (Grid.GridSize / 2),
                    0,
                    ((Size.LengthZ) / 2) * (Grid.GridSize / 2));
            }

            var location = new Vector3(
                    (gridSpaceLocation.X * Grid.GridSize),
                    0, 
                    (gridSpaceLocation.Z * Grid.GridSize))
                + buildingSizeOffset
                + Grid.Offset;

            return location;
        }
    }

}