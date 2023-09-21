using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    SerializedProperty doorTypeProp;
    SerializedProperty validKeyNameProp;
    SerializedProperty victimTransform;
    SerializedProperty doorFrameCenterTransform;
    SerializedProperty doorCloseDistance;
    SerializedProperty dotProductThreshold;

    private void OnEnable()
    {
        doorTypeProp = serializedObject.FindProperty("doorType");
        validKeyNameProp = serializedObject.FindProperty("validKeySO");
        victimTransform = serializedObject.FindProperty("victimTransform");
        doorFrameCenterTransform = serializedObject.FindProperty("doorFrameCenter");
        doorCloseDistance = serializedObject.FindProperty("doorCloseDistance");
        dotProductThreshold = serializedObject.FindProperty("dotProductThreshold");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the default inspector for all serialized fields except validKeyName and victimTransform
        DrawPropertiesExcluding(serializedObject, new string[] { "validKeySO", "victimTransform" , "doorFrameCenter" , "doorCloseDistance" , "dotProductThreshold" });

        // Show validKeyName only if doorType is KeyDoor
        if (doorTypeProp.enumValueIndex == (int)Door.DoorType.KeyDoor)
        {
            EditorGUILayout.PropertyField(validKeyNameProp);
        }
        if (doorTypeProp.enumValueIndex == (int)Door.DoorType.GhostDoor)
        {
            EditorGUILayout.PropertyField(victimTransform);
            EditorGUILayout.PropertyField(doorFrameCenterTransform);
            EditorGUILayout.PropertyField(doorCloseDistance);
            EditorGUILayout.PropertyField(dotProductThreshold);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
