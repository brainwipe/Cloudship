using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveGame
{
    public float Health { get; set;}
    public float HealthMax { get; set;}
    public float[] Position { get; set; }
    public float[] Rotation { get; set; }
    public List<BuildingSave> Buildings { get; set; }

    public SaveGame()
    {
        Buildings = new List<BuildingSave>();
    }

    [Serializable]
    public class BuildingSave
    {
        public BuildingSave(Building toSave)
        {
            Name = toSave.name;
            LocalPosition = toSave.transform.localPosition;
            LocalRotation = toSave.transform.localRotation;
            Health = toSave.Health;
        }

        public string Name;
        public Vector3 LocalPosition { get; set; }
        public Quaternion LocalRotation { get; set; }
        public float Health { get; set; }
    }
}