using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TGMap))]

public class TGMapInspector : Editor 
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        if (GUILayout.Button("Regenerate"))
        {
            TGMap tm = (TGMap)target;
            tm.BuildMesh();
        }
    }
}
