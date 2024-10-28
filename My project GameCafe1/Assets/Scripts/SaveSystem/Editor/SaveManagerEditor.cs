using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    SaveManager saveManager;
    public override void OnInspectorGUI()
    {
        saveManager = (SaveManager)target;
        if (GUILayout.Button("Find All ISaveable Objects"))
        {
            saveManager.FindAllISaveableObjects();

        }
        if(GUILayout.Button("Clear Data"))
        {
            saveManager.ClearData();
        }
        DrawDefaultInspector();
    }
}
