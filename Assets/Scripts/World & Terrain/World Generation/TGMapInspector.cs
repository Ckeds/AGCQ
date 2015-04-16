#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(WorldGenerator))]
[ExecuteInEditMode]
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
			tm.map = null;
            tm.buildWorld();
        }
        if (GUILayout.Button("Destroy All"))
        {
			WorldGenerator tm = (WorldGenerator)target;
            GameObject[] Resources = GameObject.FindGameObjectsWithTag("Resource");
            foreach (GameObject g in Resources)
                DestroyImmediate(g);
            GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
            foreach (GameObject g in maps)
                DestroyImmediate(g);
			if(tm.resources != null)
				tm.resources.Clear();
			tm.map = null;
			EditorUtility.UnloadUnusedAssetsImmediate();
        }
        if (GUILayout.Button("Clear Objects"))
        {
            GameObject[] Resources = GameObject.FindGameObjectsWithTag("Resource");
            foreach (GameObject g in Resources)
                DestroyImmediate(g);
        }
    }
}

#endif