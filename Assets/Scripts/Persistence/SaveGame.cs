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
            Id = toSave.Id;
            LocalPosition = toSave.transform.localPosition.ToArray();
            LocalRotation = toSave.transform.localRotation.ToArray();
            Health = toSave.Health;
        }

        public string Id;
        public float[] LocalPosition { get; set; }
        public float[] LocalRotation { get; set; }
        public float Health { get; set; }
    }
}
