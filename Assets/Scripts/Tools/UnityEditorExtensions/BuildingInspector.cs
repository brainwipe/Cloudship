using UnityEngine;
using System.Collections;


using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Building building = (Building)target;
        if(GUILayout.Button("Damage"))
        {
            building.Damage(10);
        }
    }
}
 