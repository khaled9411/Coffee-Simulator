using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private List<ISaveable> saveableObjects = new List<ISaveable>();

    private void Awake()
    {
        SaveSystem.SaveableObjects = saveableObjects;
        SaveSystem.LoadGame();
    }


    public void ClearData()
    {
        SaveSystem.ClearData();
    }

    public void FindAllISaveableObjects()
    {
        saveableObjects.Clear();

        MonoBehaviour[] foundObjects = GameObject.FindObjectsOfType<MonoBehaviour>(true);

        foreach (MonoBehaviour obj in foundObjects)
        {
            ISaveable saveable = obj as ISaveable;
            if (saveable != null)
            {
                saveableObjects.Add(saveable);
            }
        }

        SaveSystem.SaveableObjects = saveableObjects;

        Debug.Log($"Found {saveableObjects.Count} ISaveable objects.");
    }

    public void TestSaveSystem()
    {
        Debug.Log("=== Testing Save System ===");

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(100);
            Debug.Log("Added 100 money for testing");
        }

        SaveSystem.SaveGame();
        SaveSystem.LoadGame();
    }
}
