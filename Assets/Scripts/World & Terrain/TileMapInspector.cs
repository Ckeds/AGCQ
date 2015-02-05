using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMap))]

public class NewBehaviourScript : Editor 
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        if (GUILayout.Button("Regenerate"))
        {
            TileMap tm = (TileMap)target;
            tm.BuildMesh();
        }
    }
}
