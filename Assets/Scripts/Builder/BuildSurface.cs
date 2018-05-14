using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	Building selectedBuilding;
	BuildMenu buildMenu;
	public MeshFilter Boundary;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
	}

	void OnDisable()
	{
	}

	void OnEnable()
	{
	}
	
	void Update()
	{
		MouseMove();
		
		if (Input.GetMouseButtonDown(0))
		{
			// TODO ROLA Select
		}

		if (Input.GetMouseButton(0))
		{
			if (Input.GetKeyUp(KeyCode.Delete))
			{
				// TODO ROLA Delete
			}
		}
	
		if(Input.GetMouseButtonUp(0))
		{
			MouseUp();
		}
	}

	void MouseMove()
	{
		if (selectedBuilding == null)
		{
			selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
			SetBoundary(selectedBuilding);
		}
		else if(buildMenu.SelectedBuilding.Name != selectedBuilding.Name) 
		{
			selectedBuilding.Remove();
			selectedBuilding = null;
		}
		else
		{
			Vector3 place;
			if (GetDesired(out place))
			{
				var zeroedY = new Vector3(place.x, 0, place.z);
				selectedBuilding.transform.position = zeroedY;
				selectedBuilding.IsOverCloudship = true;
			}
			else 
			{
				selectedBuilding.IsOverCloudship = false;
			}
			selectedBuilding.UpdateVisibility();

		}
		
	}
	void MouseUp()
	{
		if (selectedBuilding != null && selectedBuilding.CanPlace)
		{
			selectedBuilding = null;
		}
	}

	bool GetDesired(out Vector3 position)
	{
		RaycastHit hit;
		position = Vector3.zero;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 8;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			position = hit.point;
			return true;
			
		}
		return false;
	}

	void SetBoundary(Building building)
	{
		float safetyMargin = 1f;

		Boundary.transform.localScale = Vector3.one;
		var boundaryExtents = Boundary.mesh.bounds.extents;
		var buildingMeshFilter = building.GetComponent<MeshFilter>();
		var buildingSizes = (buildingMeshFilter.mesh.bounds.extents + new Vector3(safetyMargin, 0, safetyMargin));

		var xScale = (boundaryExtents.x - buildingSizes.x) / boundaryExtents.x;
		var zScale = (boundaryExtents.z - buildingSizes.z) / boundaryExtents.z;
		Boundary.transform.localScale = new Vector3(xScale, 1, zScale);
	}
}
