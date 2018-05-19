using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour 
{
	Building selectedBuilding;
	BuildMenu buildMenu;
	public MeshFilter Boundary;
	public Vector3 buildingToGrabPointOffset = Vector3.zero;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
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
					buildingToGrabPointOffset = FindGrabPoint(selectedBuilding);
				}
				else
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
					buildingToGrabPointOffset = Vector3.zero;
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
			buildingToGrabPointOffset = Vector3.zero;
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
			selectedBuilding.transform.position = place;
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
			if (hit.transform.tag == "Building")
			{
				building = hit.transform.GetComponentInParent<Building>();
				return true;
			}
			else
			{
				Debug.Log(hit.transform.tag + " " + hit.transform.gameObject.name);
			}
			return false;
		}
		return false;
	}

	bool GetDesired(out Vector3 position)
	{
		position = Vector3.zero;
		RaycastHit hit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		var distance = ((selectedBuilding.transform.position + buildingToGrabPointOffset) - mouseRay.origin).magnitude;
		mouseRay.direction = (mouseRay.direction * distance) - buildingToGrabPointOffset;

		int layerMask = 1 << 8;

		if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, layerMask))
		{
			position = Vector3.Lerp(
				selectedBuilding.transform.position, 
				hit.point, 
				Time.deltaTime * 40); 

			return true;
		}
		return false;
	}

	Vector3 FindGrabPoint(Building selectedBuilding)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			return hit.point - selectedBuilding.transform.position;
		}
		return Vector3.zero;
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
