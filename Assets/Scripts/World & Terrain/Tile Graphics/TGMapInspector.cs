using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TGMap))]

public class TGMapInspector : Editor 
{

    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();
        DrawDefaultInspector();
        if (GUILayout.Button("Regenerate"))
        {
            GameObject[] Resources = GameObject.FindGameObjectsWithTag("Resource");
            foreach (GameObject g in Resources)
                DestroyImmediate(g);
            TGMap tm = (TGMap)target;
            tm.BuildMesh();
        }
    }
}

