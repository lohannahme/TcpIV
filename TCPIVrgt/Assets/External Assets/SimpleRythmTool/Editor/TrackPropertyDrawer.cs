using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(Track))]
public class TrackPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //property.serializedObject.Update();
        SerializedProperty _beats = property.FindPropertyRelative("beats");
        SerializedProperty _name = property.FindPropertyRelative("name");

        EditorGUILayout.BeginHorizontal();

        Rect nameRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing, EditorGUIUtility.labelWidth - 10, EditorGUIUtility.singleLineHeight);
        _name.stringValue = EditorGUI.TextField(nameRect, _name.stringValue);
        Rect beatsRect = new Rect(position.x + nameRect.width, position.y, position.width - nameRect.width, nameRect.height);
        for (int i = 0; i < _beats.arraySize; i++)
        {
            /*_beats.GetArrayElementAtIndex(i).boolValue = *///EditorGUI.PropertyField(new Rect(position.x + 20 * i, position.y, 20, 20), _beats.GetArrayElementAtIndex(i));
            //_beats.GetArrayElementAtIndex(i).boolValue = EditorGUI.Toggle(new Rect(position.x + 20 * i, position.y, 20, 20), _beats.GetArrayElementAtIndex(i).boolValue);
            EditorGUI.PropertyField(new Rect(beatsRect.x + 25 * i + 5 + 5, beatsRect.y + 2, 25, beatsRect.height),
                                    _beats.GetArrayElementAtIndex(i), GUIContent.none);
        }

        EditorGUILayout.EndHorizontal();

        SerializedProperty _event = property.FindPropertyRelative("OnBeat");
        SerializedProperty foldout = property.FindPropertyRelative("foldout");
        foldout.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(foldout.boolValue, "event");
        if (foldout.boolValue)
        {
            //EditorGUI.PropertyField(new Rect(position.x, position.y + 20, 200, 100), _event, GUIContent.none);
            EditorGUILayout.PropertyField(_event);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        //base.OnGUI(position, property, label);
        //property.serializedObject.ApplyModifiedProperties();
        //position = total;
    }
}
