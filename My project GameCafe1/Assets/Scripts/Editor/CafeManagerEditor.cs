using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(CafeManager))]
public class CafeManagerEditor : Editor
{
    private ReorderableList areaItemsList;

    private void OnEnable()
    {
        areaItemsList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("areaItemsList"),
            true, true, true, true);

        areaItemsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Area Items List");
        };

        areaItemsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = areaItemsList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty areaProperty = element.FindPropertyRelative("area");
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                areaProperty,
                new GUIContent("Area")
            );

            SerializedProperty itemsProperty = element.FindPropertyRelative("items");
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUI.GetPropertyHeight(itemsProperty)),
                itemsProperty,
                new GUIContent("Items"),
                true
            );

            // Call to validate items
            ValidateItems(itemsProperty);
        };

        areaItemsList.elementHeightCallback = (index) =>
        {
            var element = areaItemsList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty itemsProperty = element.FindPropertyRelative("items");

            float itemsHeight = EditorGUI.GetPropertyHeight(itemsProperty);
            return EditorGUIUtility.singleLineHeight + itemsHeight + 10;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        areaItemsList.DoLayoutList();

        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true);
        while (property.NextVisible(false))
        {
            if (property.name != "areaItemsList")
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ValidateItems(SerializedProperty itemsProperty)
    {
        for (int i = itemsProperty.arraySize - 1; i >= 0; i--)
        {
            SerializedProperty item = itemsProperty.GetArrayElementAtIndex(i);

            // Check if item implements IBuyableRespondable
            if (item.objectReferenceValue != null && !(item.objectReferenceValue is IBuyableRespondable))
            {
                // Set item to null if it does not implement IBuyableRespondable
                item.objectReferenceValue = null;
            }
        }
    }
}
