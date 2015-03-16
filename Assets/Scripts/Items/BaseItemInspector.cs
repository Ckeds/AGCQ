/*using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BaseItem))]

public class BaseItemInspector : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Regenerate"))
		{
			BaseItem bI = (BaseItem)target;
			bI.CreateGUITex();
			bI.CreateGUITexHover();
		}

	}
}*/