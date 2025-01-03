using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/savefile.json";
    public static List<ISaveable> SaveableObjects { get; set; } = new List<ISaveable>();

    public static void SaveGame()
    {
        RefreshSaveableObjects();
        if (SaveableObjects == null || SaveableObjects.Count == 0)
        {
            Debug.LogError("No ISaveable objects found to save.");
            return;
        }

        //try
        //{
            List<GameData> allData = new List<GameData>();
            foreach (ISaveable saveable in SaveableObjects)
            {
                var saveData = saveable.SaveData();
                string jsonData = JsonUtility.ToJson(saveData);

                Debug.Log(saveable);
                allData.Add(new GameData
                {
                    uniqueID = saveable.UniqueID,
                    typeName = saveable.GetType().FullName,
                    dataType = saveData.GetType().FullName,
                    jsonData = jsonData
                });
                Debug.Log($"Saved data for {saveable.GetType().Name} (ID: {saveable.UniqueID}): {jsonData}");
            }

            string finalJson = JsonUtility.ToJson(new SerializationWrapper { allData = allData });
            File.WriteAllText(saveFilePath, finalJson);
            Debug.Log($"Game Saved successfully. Saved {SaveableObjects.Count} objects.");
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError($"Error saving game: {e.Message}");
        //}
    }

    public static void ClearData()
    {
        try
        {
            File.Delete(saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error clearing game data: {e.Message}");
        }
    }

    public static void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        try
        {
            RefreshSaveableObjects();
            string json = File.ReadAllText(saveFilePath);
            var saveWrapper = JsonUtility.FromJson<SerializationWrapper>(json);

            if (saveWrapper.allData == null || saveWrapper.allData.Count == 0)
            {
                Debug.LogError("Save file is empty or corrupted.");
                return;
            }

            foreach (var gameData in saveWrapper.allData)
            {
                foreach (var saveable in SaveableObjects)
                {
                    if (saveable.UniqueID == gameData.uniqueID)
                    {
                        try
                        {
                            System.Type dataType = System.Type.GetType(gameData.dataType);
                            if (dataType != null)
                            {
                                SaveData loadedData = (SaveData)JsonUtility.FromJson(gameData.jsonData, dataType);
                                saveable.LoadData(loadedData);
                                Debug.Log($"Loaded data for {gameData.typeName} (ID: {gameData.uniqueID}): {gameData.jsonData}");
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"Error loading data for {gameData.typeName} (ID: {gameData.uniqueID}): {e.Message}");
                        }
                        break;
                    }
                }
            }
            Debug.Log("Game Loaded Successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }
    }

    private static void RefreshSaveableObjects()
    {
        var foundObjects = GameObject.FindObjectsOfType<MonoBehaviour>(true);
        SaveableObjects.Clear();
        foreach (MonoBehaviour obj in foundObjects)
        {
            if (obj is ISaveable saveable)
            {
                SaveableObjects.Add(saveable);
            }
        }
    }

    [System.Serializable]
    private class GameData
    {
        public string uniqueID;
        public string typeName;
        public string dataType;
        public string jsonData;
    }

    [System.Serializable]
    private class SerializationWrapper
    {
        public List<GameData> allData;
    }

}