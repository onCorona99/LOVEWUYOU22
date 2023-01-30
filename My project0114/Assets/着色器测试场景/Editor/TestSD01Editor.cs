using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(TestSD01))]
public class TestSD01Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("ÇÐ»»SD"))
        {
            (target as TestSD01).ShiftSD();
        }

        GUILayout.EndHorizontal();

    }
}
