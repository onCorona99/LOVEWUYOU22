using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(RampManager))]
public class RampManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Éú³ÉRampTex"))
        {
            (target as RampManager).GenerateRampTex();
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
