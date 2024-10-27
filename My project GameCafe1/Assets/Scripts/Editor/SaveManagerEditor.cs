using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Find All ISaveable Objects"))
        {
            SaveManager saveManager = (SaveManager)target;
            saveManager.FindAllISaveableObjects();
        }
        if(GUILayout.Button("Clear Data"))
        {
            SaveManager saveManager = (SaveManager) target;
            saveManager.ClearData();
        }
        DrawDefaultInspector();
    }
}
