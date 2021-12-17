using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sheet))]
public class SheetEditor : Editor
{
    Sheet _sheet;
    Vector2 scrollPos;

    private void OnEnable()
    {
        _sheet = (Sheet)target;
        _sheet.Validate();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        /*_sheet.SetBPM(IntField(_sheet.BPM, "BPM", true));
        _sheet.SetTrackCount(IntField(_sheet.trackCount, "Tracks", false));
        _sheet.SetBeatCount(IntField(_sheet.beatCount, "Beats", false));*/
        _sheet.BPM = IntField(_sheet.BPM, "BPM", true);
        _sheet.trackCount = IntField(_sheet.trackCount, "Tracks", false);
        _sheet.beatCount = IntField(_sheet.beatCount, "Beats", false);

        if (EditorGUI.EndChangeCheck())
            _sheet.Validate();

        SerializedProperty prop = serializedObject.FindProperty("tracks");
        
        GUI.enabled = false;
        for (int i = 0; i < prop.arraySize; i++)
        {
            EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), GUIContent.none);
        }
        GUI.enabled = true;

        if (GUILayout.Button("Editor Window"))
        {
            SheetWindow.Show(_sheet);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private int IntField(int value, string label, bool enable = true)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);

        GUI.enabled = enable;
        value = EditorGUILayout.IntField(value);
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();
        return value;
    }

    private int IntFieldControll(int value, string label, bool enable = true)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel(label);

        if (GUILayout.Button("-") && value > 0)
            value--;

        GUI.enabled = enable;
        //EditorGUI... permite personalizar o layout
        value = EditorGUILayout.IntField(value);
        GUI.enabled = true;
        
        if (GUILayout.Button("+"))
            value++;

        EditorGUILayout.EndHorizontal();
        return value;
    }
}
