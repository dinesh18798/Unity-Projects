using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    private static readonly string path = Application.persistentDataPath + "/player.bin";

    public static void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData();
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static bool CheckLoadFile()
    {
        return File.Exists(path);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            GameData gameData = binaryFormatter.Deserialize(fileStream) as GameData;
            fileStream.Close();

            return gameData;
        }
        else
        {
#if DEBUG
            Debug.LogError("Save file not found: " + path);
#endif

            return null;
        }
    }
}
