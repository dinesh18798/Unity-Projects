using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadManager
{
    private static readonly string gameDataPath = Application.persistentDataPath + "/gamedata.bin";
    private static readonly string playerDataPath = Application.persistentDataPath + "/playerdata.bin";

    internal static void SavePlayerData()
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(playerDataPath, FileMode.OpenOrCreate);

            PlayerData playerData = new PlayerData();
            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
        }
        catch (Exception exp)
        {
            Debug.Log("Unable to save player data" + exp.StackTrace);
        }
    }

    internal static bool CheckPlayerDataExist()
    {
        return File.Exists(playerDataPath);
    }

    internal static PlayerData LoadPlayerData()
    {
        if (File.Exists(playerDataPath))
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(playerDataPath, FileMode.Open);

                PlayerData playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
                fileStream.Close();

                return playerData;
            }
            catch (Exception exp)
            {
                Debug.Log("Unable to load the player data" + exp.StackTrace);
            }
        }
        else
        {
            Debug.LogError("Saves file not found: " + playerDataPath);
        }
        return null;
    }

    internal static void SaveGameData()
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(gameDataPath, FileMode.OpenOrCreate);

            GameData gameData = new GameData();
            binaryFormatter.Serialize(fileStream, gameData);
            fileStream.Close();
        }
        catch (Exception exp)
        {
            Debug.Log("Unable to save player data" + exp.StackTrace);
        }
    }

    internal static bool CheckGameDataExist()
    {
        return File.Exists(gameDataPath);
    }

    internal static GameData LoadGameData()
    {
        if (File.Exists(gameDataPath))
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(gameDataPath, FileMode.Open);

                GameData gameData = binaryFormatter.Deserialize(fileStream) as GameData;
                fileStream.Close();

                return gameData;
            }
            catch (Exception exp)
            {
                Debug.Log("Unable to load the game data" + exp.StackTrace);
            }
        }
        else
        {
            Debug.LogError("Saves file not found: " + gameDataPath);
        }
        return null;
    }
}
