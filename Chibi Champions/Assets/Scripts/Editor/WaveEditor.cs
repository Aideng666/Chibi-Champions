using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WaveManager))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WaveManager manager = (WaveManager)target;

        GUILayout.Space(10);

        GUILayout.BeginVertical();

            for (int i = 0; i < manager.GetWaveCount(); i++)
            {
                GUILayout.Label("Enemies in wave " + (i + 1));

                SerializedProperty enemiesLists = serializedObject.FindProperty("enemiesLists");
            }

        GUILayout.EndVertical();
    }
}
