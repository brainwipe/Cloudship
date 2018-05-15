using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	enum BuildMode {
		Select,
		Place
	}

	Building selectedBuilding;
	BuildMenu buildMenu;
	public MeshFilter Boundary;
	BuildMode mode;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
		mode = BuildMode.Select;
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
			if (selectedBuilding == null)
			{
				Building building;
				if (IsThereABuildingUnderMousePointer(out building))
				{

					selectedBuilding = building;
				}
				else
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
				}
				SetBoundary(selectedBuilding);
			}
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
			PlaceBuilding();
		}
	}

	void MouseMove()
	{
		if (selectedBuilding == null)
		{
			return;
		}	

		if(buildMenu.SelectedBuilding.Name != selectedBuilding.Name) 
		{
			selectedBuilding.Remove();
			selectedBuilding = null;
		}

		MoveSelectedBuilding();
	}

	void PlaceBuilding()
	{
		if (selectedBuilding != null && !selectedBuilding.CanPlace)
		{
			selectedBuilding.Remove();
		}
		selectedBuilding = null;
	}

	void MoveSelectedBuilding()
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

	bool IsThereABuildingUnderMousePointer(out Building building)
	{
		RaycastHit hit;
		building = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			Debug.Log(hit.transform.tag);
			if (hit.transform.tag == "Building")
			{
				building = hit.transform.GetComponentInParent<Building>();
				return true;
			}
			return false;
		}
		return false;
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
