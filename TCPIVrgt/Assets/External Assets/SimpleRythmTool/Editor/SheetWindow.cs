using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SheetWindow : EditorWindow
{
    public static SheetWindow window;
    private static Sheet target;
    Vector2 scrollPos;

    public static void Show(Sheet target)
    {
        SheetWindow.target = target;
        window = GetWindow<SheetWindow>();
        window.titleContent = new GUIContent("Simple Rythm Tool");
    }

    private void OnGUI()
    {
        target.BPM = IntField(target.BPM, "BPM");
        target.trackCount = IntField(target.trackCount, "Tracks", false);
        target.beatCount = IntField(target.beatCount, "Beats", false);

        target.Validate();

        EditorGUILayout.LabelField("");

        SerializedObject so = new SerializedObject(target);
        so.Update();
        SerializedProperty sp = so.FindProperty("tracks");

        float posY = EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 6;
        float viewRectH = EditorGUI.GetPropertyHeight(sp) * sp.arraySize;
        float viewRectW = 20 * (target.beatCount + 1);

        Vector2 scrl = GUI.BeginScrollView(new Rect(0, posY, EditorGUIUtility.currentViewWidth, window.rootVisualElement.contentRect.height - posY),
                                           new Vector2(scrollPos.x, 0), new Rect(0, posY, viewRectW, 20));
        scrollPos.x = scrl.x;

        if (sp.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < target.beatCount; j++)
            {
                EditorGUI.LabelField(new Rect(EditorGUIUtility.standardVerticalSpacing + j * 20, posY, 20, 20), "" + (j + 1));
            }
            EditorGUILayout.EndHorizontal();
            posY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
        }

        GUI.EndScrollView();
        scrollPos = GUI.BeginScrollView(new Rect(0, posY, EditorGUIUtility.currentViewWidth, window.rootVisualElement.contentRect.height - posY),
                                        scrollPos, new Rect(0, posY, viewRectW, viewRectH));

        for (int i = 0; i < target.tracks.Length; i++)
        {
            SerializedProperty pista = sp.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(pista);
        }

        GUI.EndScrollView();

        so.ApplyModifiedProperties();
    }

    private int IntField(int value, string label, bool enable = true)
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
