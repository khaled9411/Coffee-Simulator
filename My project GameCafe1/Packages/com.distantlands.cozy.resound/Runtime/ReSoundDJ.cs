// Distant Lands 2022.


using System;
using DistantLands.Cozy.Data;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



namespace DistantLands.Cozy.Data
{

    [System.Serializable]
    [CreateAssetMenu(menuName = "Distant Lands/Cozy/ReSound/DJ", order = 361)]
    public class ReSoundDJ : ScriptableObject
    {

        public enum TransitionType
        {
            fadeToZero,
            crossfade,
            noFade
        }
        public TransitionType transitionType;
        public float transitionTime = 5;

        [Tooltip("All of the ReSound tracks will play ever.")]
        public List<ReSoundTrack> availableTracks;
        public bool resetOnEntry;
        public bool noSilenceMode;

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ReSoundDJ))]
    [CanEditMultipleObjects]
    public class E_ReSoundDJ : Editor
    {

        SerializedProperty availableTracks;
        SerializedProperty transitionType;
        SerializedProperty transitionTime;
        SerializedProperty resetOnEntry;
        SerializedProperty noSilenceMode;
        ReSoundDJ prof;
        Vector2 scrollPos;

        void OnEnable()
        {
            availableTracks = serializedObject.FindProperty("availableTracks");
            transitionType = serializedObject.FindProperty("transitionType");
            transitionTime = serializedObject.FindProperty("transitionTime");
            resetOnEntry = serializedObject.FindProperty("resetOnEntry");
            noSilenceMode = serializedObject.FindProperty("noSilenceMode");
            prof = (ReSoundDJ)target;

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(transitionType);
            EditorGUILayout.PropertyField(transitionTime);
            EditorGUILayout.PropertyField(resetOnEntry);
            EditorGUILayout.PropertyField(noSilenceMode);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(availableTracks);

            serializedObject.ApplyModifiedProperties();


        }

    }
#endif
}