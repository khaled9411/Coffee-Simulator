using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEditor;

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
                NewID(saveable as MonoBehaviour);
            }
        }

        SaveSystem.SaveableObjects = saveableObjects;

        Debug.Log($"Found {saveableObjects.Count} ISaveable objects.");
    }

    private void NewID(Object obj)
    {
        SerializedObject serializedObject = new SerializedObject(obj);
        SerializedProperty idProperty = serializedObject.FindProperty("uniqueID");

        if (idProperty != null)
        {
            idProperty.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(obj);
        }
        else
        {
            Debug.LogWarning($"Property 'uniqueID' not found in {obj.name}. Make sure it's defined as [SerializeField].");
        }
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
