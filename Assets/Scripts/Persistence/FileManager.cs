using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string saveGameName = "cloudshipSaveGame.dat";

    private string SavePath => Path.Combine(Application.persistentDataPath, saveGameName);

    public void Save()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        var bf = new BinaryFormatter();
        FileStream file = File.Create(SavePath);
        var save = CreateSave();
        bf.Serialize(file, save);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(SavePath, FileMode.Open);
            var save = (SaveGame)bf.Deserialize(file);
            file.Close();

            ApplySave(save);
        }
    }

    void ApplySave(SaveGame save)
    {
        var saveables = FindObjectsOfType<MonoBehaviour>().OfType<IAmPersisted>();
        Debug.Log("load saveable count: " + saveables.Count());
        foreach(var saveable in saveables)
        {
            saveable.Load(save);
        }
    }

    SaveGame CreateSave()
    {
        var saveables = FindObjectsOfType<MonoBehaviour>().OfType<IAmPersisted>();
        Debug.Log("save saveable count: " + saveables.Count());
        var save = new SaveGame();
        foreach(var saveable in saveables)
        {
            saveable.Save(save);
        }
        return save;
    }
}