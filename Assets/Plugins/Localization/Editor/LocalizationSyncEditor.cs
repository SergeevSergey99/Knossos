using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LocalizationSync))]
public class LocalizationSyncEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Sync")) 
        {
            var mg = (target as LocalizationSync);
            mg.Sync();
        }
        if (GUILayout.Button("Spawn")) 
        {
            var mg = (target as LocalizationSync);
            mg.Spawn();
        }
    }
}
