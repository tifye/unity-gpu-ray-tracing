using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RayTracingMaster))]
public class SceneEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RayTracingMaster master = (RayTracingMaster)target;
        if (GUILayout.Button("Generate Scene")) {
            master.ResetScene();
        }
    }
}
