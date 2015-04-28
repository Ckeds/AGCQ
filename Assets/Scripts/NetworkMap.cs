using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMap : MonoBehaviour {

	private int[,] map;
	public int[,] Map
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
			//map = wg.map.mapData;
			resources = wg.resources;
			int size = wg.MapUnitySize;
			string mapString = "";
			int startX = 0;
			int startY = 0;
			Debug.Log(size);
			for(int y = 0; y < size; y++)
			{
				for(int x = 0; x < size; x++) 
				{
					string thisInt = ""+wg.map.mapData[x,y];
					mapString += thisInt;
					if(mapString.Length >= 4000)
					{
						this.GetComponent<NetworkView>().RPC("SetMap",RPCMode.AllBuffered, mapString, startX, startY, size);
						mapString = "";
						startY = y;
						startX = x + 1;
						if(startX >= size)
						{
							startY++;
							startX = 0;
						}
					}
				}
			}
			//Debug.Log(mapString.Length);
			this.GetComponent<NetworkView>().RPC("SetMap",RPCMode.AllBuffered, mapString, startX, startY, size);
			Debug.Log(map.Length);
			Debug.Log(wg.map.mapData.Length);
			/*for(int checkY = 0; checkY < size; checkY++)
			{
				for(int checkX = 0; checkX < size; checkX++)
				{
					//Debug.Log(checkX);
					//Debug.Log(size);
					bool doublecheck = (map[checkX,checkY] == wg.map.mapData[checkX,checkY]);
					Debug.Log(doublecheck);
				}
			}*/
		}
	}
	[RPC]
	void SetMap(string m, int startx, int starty, int size)
	{
		Debug.Log ("RPC");
		if (map == null)
		{
			map = new int[size,size];
		}
		int x = startx;
		int y = starty;
		foreach(char c in m)
		{
			map[x,y] = int.Parse(c.ToString());
			x++;
			if(x >= size)
			{
				x = 0;
				y++;
			}
		}
	}
}
