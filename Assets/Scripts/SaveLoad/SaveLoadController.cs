using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadController
{
    private const string SaveFileExtension = "projzombie";

    #region Temporary Data

    public static void SaveTempData(TempSaveData tempData)
    {
        // Path to save file
        var path = $"{Application.persistentDataPath}/tempSave.{SaveFileExtension}";

        // Initialize formatter & file stream
        var binaryFormatter = new BinaryFormatter();
        var fileStream = new FileStream(path, FileMode.Create);

        // Serialize & save data to file
        binaryFormatter.Serialize(fileStream, tempData);
        fileStream.Close();
    }

    public static TempSaveData LoadTempData()
    {
        // Path to save file
        var path = $"{Application.persistentDataPath}/tempSave.{SaveFileExtension}";
        if (!File.Exists(path))
        {
            return null;
        }

        // Initialize formatter & file stream
        var binaryFormatter = new BinaryFormatter();
        var fileStream = new FileStream(path, FileMode.Open);

        // Load & return temp data file
        var tempData = binaryFormatter.Deserialize(fileStream) as TempSaveData;
        fileStream.Close();

        return tempData;
    }

    public static void DeleteTempData()
    {
        // Path to save file
        var path = $"{Application.persistentDataPath}/tempSave.{SaveFileExtension}";
        if (File.Exists(path)) File.Delete(path);
    }

    #endregion
}
