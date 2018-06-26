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

            // TODO ApplySave
        }
    }

    SaveGame CreateSave()
    {
        var saveables = FindObjectsOfType<MonoBehaviour>().OfType<IAmPersisted>();
        var save = new SaveGame();
        foreach(var saveable in saveables)
        {
            saveable.Save(save);
        }
        return save;
    }
}