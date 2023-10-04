using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GatherableSO))]
public class GatherableObjectSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GatherableSO gatherableSO = (GatherableSO)target;

        // Draw the default fields
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gatherableObjectPrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gatherableImageSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gatherableObjectName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gatherableType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("storingType"));

        // Check if the GatherableObjectType is Usable
        if (gatherableSO.gatherableType == GatherableObjectType.Usable || gatherableSO.gatherableType == GatherableObjectType.Healable)
        {
            // Display the 'value' field only when the type is Usable
            EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
