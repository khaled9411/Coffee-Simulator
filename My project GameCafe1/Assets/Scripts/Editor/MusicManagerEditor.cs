using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor
{
    float volume;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MusicManager musicManager = (MusicManager)target;

        volume = EditorGUILayout.FloatField("Volume", volume);
        if (GUILayout.Button("Run Setvolume"))
        {
            musicManager.SetVolume(volume);
        }
        if (GUILayout.Button("StartMusic"))
        {
            musicManager.StartMusic();
        }
        if (GUILayout.Button("StopMusic"))
        {
            musicManager.StopMusic();
        }
    }
}
