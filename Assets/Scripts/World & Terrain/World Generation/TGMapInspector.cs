using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(WorldGenerator))]

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
            GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
            foreach (GameObject g in maps)
                DestroyImmediate(g);
            WorldGenerator tm = (WorldGenerator)target;
            tm.buildWorld();
        }
        if (GUILayout.Button("Destroy"))
        {
            GameObject[] Resources = GameObject.FindGameObjectsWithTag("Resource");
            foreach (GameObject g in Resources)
                DestroyImmediate(g);
            GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
            foreach (GameObject g in maps)
                DestroyImmediate(g);
        }
    }
}

