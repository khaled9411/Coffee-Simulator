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

        try
        {
            List<GameData> allData = new List<GameData>();
            foreach (ISaveable saveable in SaveableObjects)
            {
                var saveData = saveable.SaveData();
                string jsonData = JsonUtility.ToJson(saveData);
                allData.Add(new GameData
                {
                    typeName = saveable.GetType().FullName,
                    dataType = saveData.GetType().FullName,
                    jsonData = jsonData
                });
                Debug.Log($"Saved data for {saveable.GetType().Name}: {jsonData}");
            }

            string finalJson = JsonUtility.ToJson(new SerializationWrapper { allData = allData });
            File.WriteAllText(saveFilePath, finalJson);
            Debug.Log($"Game Saved successfully. Saved {SaveableObjects.Count} objects.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
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
                    if (saveable.GetType().FullName == gameData.typeName)
                    {
                        try
                        {
                            System.Type dataType = System.Type.GetType(gameData.dataType);
                            if (dataType != null)
                            {
                                SaveData loadedData = (SaveData)JsonUtility.FromJson(gameData.jsonData, dataType);
                                saveable.LoadData(loadedData);
                                Debug.Log($"Loaded data for {gameData.typeName}: {gameData.jsonData}");
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"Error loading data for {gameData.typeName}: {e.Message}");
                        }
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