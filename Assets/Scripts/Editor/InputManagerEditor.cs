using System.Collections;
using UnityEditor;
using UnityEngine;
using static InputManager;


[CustomEditor(typeof(InputManager))] // Replace 'YourComponent' with the actual script name
public class InputManagerEditor : Editor
{
    SerializedProperty examineObjectZoomTypeProp;
    SerializedProperty zoomPlusKeysProp;
    SerializedProperty downMinusKeysProp;

    void OnEnable()
    {
        examineObjectZoomTypeProp = serializedObject.FindProperty("examineObjectZoomType");
        zoomPlusKeysProp = serializedObject.FindProperty("ZoomPlusKeys");
        downMinusKeysProp = serializedObject.FindProperty("ZoomMinusKeys");
    }
    public override void OnInspectorGUI()
    {
        InputManager inputManager = (InputManager)target;

        inputManager.examineControlType = (ExamineObjectRotateType)EditorGUILayout.EnumPopup("Examine Control Type", inputManager.examineControlType);

        // Show the KeyCode fields only if the control type is KeyPadControl
        if (inputManager.examineControlType == ExamineObjectRotateType.KeyPadControl)
        {
            EditorGUILayout.LabelField("Keys for Object Examination");
            EditorGUILayout.Space();

            DrawKeyCodeArray("Up Direction Keys", ref inputManager.UpDirKeys);
            DrawKeyCodeArray("Down Direction Keys", ref inputManager.DownDirKeys);
            DrawKeyCodeArray("Left Direction Keys", ref inputManager.LeftDirKeys);
            DrawKeyCodeArray("Right Direction Keys", ref inputManager.RightDirKeys);
        }

        EditorGUILayout.PropertyField(examineObjectZoomTypeProp);

        // Show/hide the zoom-related fields based on the selected examineObjectZoomType
        if (inputManager.examineObjectZoomType == ExamineObjectZoomType.MouseControl)
        {
            // Hide zoom-related fields
            zoomPlusKeysProp.isExpanded = false;
            downMinusKeysProp.isExpanded = false;
        }
        else if (inputManager.examineObjectZoomType == ExamineObjectZoomType.KeyPadControl)
        {
            // Show zoom-related fields
            zoomPlusKeysProp.isExpanded = true;
            downMinusKeysProp.isExpanded = true;

            EditorGUILayout.PropertyField(zoomPlusKeysProp, true);
            EditorGUILayout.PropertyField(downMinusKeysProp, true);
        }

        serializedObject.ApplyModifiedProperties();

        // Draw the rest of your fields or properties here

        if (GUI.changed)
        {
            EditorUtility.SetDirty(inputManager);
        }
    }


    private void DrawKeyCodeArray(string label, ref KeyCode[] keys)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        int newSize = EditorGUILayout.IntField(keys.Length);
        if (newSize != keys.Length)
        {
            System.Array.Resize(ref keys, newSize);
        }
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = (KeyCode)EditorGUILayout.EnumPopup(keys[i]);
        }
        EditorGUILayout.EndHorizontal();
    }

}
