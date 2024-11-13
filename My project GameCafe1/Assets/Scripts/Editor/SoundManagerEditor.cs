using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    float volume;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundManager soundManager = (SoundManager)target;

        volume = EditorGUILayout.FloatField("Volume", volume);
        if (GUILayout.Button("Run Setvolume"))
        {
            soundManager.SetVolume(volume);
        }
    }
}
