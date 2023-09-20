using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    SerializedProperty doorTypeProp;
    SerializedProperty validKeyNameProp;
    SerializedProperty victimTransform;

    private void OnEnable()
    {
        doorTypeProp = serializedObject.FindProperty("doorType");
        validKeyNameProp = serializedObject.FindProperty("validKeySO");
        victimTransform = serializedObject.FindProperty("victimTransform");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the default inspector for all serialized fields except validKeyName and victimTransform
        DrawPropertiesExcluding(serializedObject, new string[] { "validKeySO", "victimTransform" });

        // Show validKeyName only if doorType is KeyDoor
        if (doorTypeProp.enumValueIndex == (int)Door.DoorType.KeyDoor)
        {
            EditorGUILayout.PropertyField(validKeyNameProp);
        }
        if (doorTypeProp.enumValueIndex == (int)Door.DoorType.GhostDoor)
        {
            EditorGUILayout.PropertyField(victimTransform);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
