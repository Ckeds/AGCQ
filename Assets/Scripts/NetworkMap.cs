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
	WorldGenerator wg;
	// Use this for initialization
	void Awake () {
		wg = GameObject.Find ("GameManagerGO").GetComponent<WorldGenerator> ();
		Debug.Log (wg);
		if(this.GetComponent<NetworkView>().isMine)
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
						this.GetComponent<NetworkView>().RPC("SetMap",RPCMode.OthersBuffered, mapString, startX, startY, size);
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
			Debug.Log(mapString.Length);
			this.GetComponent<NetworkView>().RPC("SetMap",RPCMode.OthersBuffered, mapString, startX, startY, size);
			Debug.Log(map.Length);
			//Debug.Log(wg.map.mapData.Length);
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
			string resource;
			for(int i = 0; i < resources.Count; i++)
			{
				resource = "";
				foreach (Resource r in resources[i])
				{
					int x = (int)r.position.x;
					int y = (int)r.position.y;
					if(x < 10)
					{
						resource+="0";
					}
					resource+= ""+x;
					if(y < 10)
					{
						resource+="0";
					}
					resource+= ""+y;
					resource+= r.type;
				}
				//Debug.Log(resource);
				this.GetComponent<NetworkView>().RPC("SetResources", RPCMode.OthersBuffered, resource);
			}
			this.GetComponent<NetworkView>().RPC("CreateWorld", RPCMode.OthersBuffered, wg.mapSizeName);
			Camera.main.GetComponent<FollowCamera>().target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}
		else 
		{
			wg.resources = null;
			wg.map.mapData = null;
			Debug.Log (wg.resources);
		}
	}
	void Test()
	{
		Debug.Log ("Here");
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
		Debug.Log (m.Length);
	}
	[RPC]
	void SetResources(string r)
	{
		Debug.Log ("RPC2");
		List<Resource> resource = new List<Resource> ();
		int chunks = 5;
		int stringLength = r.Length;
		for(int i = 0; i < stringLength; i += chunks)
		{
			string resourceString = r.Substring(i, chunks);
			float x = float.Parse(resourceString.Substring(0,2));
			float y = float.Parse(resourceString.Substring(2,2));
			char type = resourceString[resourceString.Length -1];
			Resource re = new Resource();
			re.position = new Vector2(x,y);
			re.type = type;
			resource.Add(re);
		}
		if(resources == null)
		{
			resources = new List<List<Resource>>();
		}
		resources.Add (resource);
	}
	[RPC]
	void CreateWorld(string size)
	{
		Debug.Log ("MAPRPC");
		wg.map.mapData = map;
		wg.resources = resources;
		wg.mapSizeName = size;
		wg.StartBuild();
		Test ();
	}
}
