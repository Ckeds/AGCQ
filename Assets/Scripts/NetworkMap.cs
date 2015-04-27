using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMap : MonoBehaviour {

	private byte[,] map;
	public byte[,] Map
	{
		get{ return map;}
	}
	private List<List<Resource>> resources;
	public List<List<Resource>> Resources
	{
		get{ return resources;}
	}
	// Use this for initialization
	void Awake () {
		WorldGenerator wg = GameObject.Find ("GameManagerGO").GetComponent<WorldGenerator> ();
		Debug.Log (wg);
		if(!this.GetComponent<NetworkView>().isMine)
		{
			Debug.Log("GONNA WORK");
			Debug.Log(resources);
			wg.map.mapData = map;
			wg.resources = resources;
			StartCoroutine(wg.buildWorld());
		}
		else
		{
			Debug.Log("SETUP");
			SetMap(wg.map.mapData, wg.resources);
		}
	}
	void SetMap(byte[,] m, List<List<Resource>> r)
	{
		map = m;
		resources = r;
	}
}
