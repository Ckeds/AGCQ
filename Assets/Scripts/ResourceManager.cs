using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {

	public GameObject map;
	public GameObject rock;
	public GameObject pine;
	public GameObject oak;
	public GameObject dirt;
	public GameObject sand;
	public GameObject water;

	List<GameObject> maps;
	List<GameObject> rocks;
	List<GameObject> pines;
	List<GameObject> oaks;
	List<GameObject> dirts;
	List<GameObject> sands;
	List<GameObject> waters;
	// Use this for initialization
	public void Setup (int m, int r, int p, int o, int d, int s, int w) 
	{
		maps = new List<GameObject> ();
		rocks = new List<GameObject> ();
		pines = new List<GameObject> ();
		oaks = new List<GameObject> ();
		dirts = new List<GameObject> ();
		sands = new List<GameObject> ();
		waters = new List<GameObject> ();
		
		int i;
		for(i = 0; i < m; i++)
		{
			GameObject obj = (GameObject)Instantiate(map);
			obj.SetActive(false);
			maps.Add(obj);
		}
		for(i = 0; i < r; i++)
		{
			GameObject obj = (GameObject)Instantiate(rock);
			obj.SetActive(false);
			rocks.Add(obj);
		}
		for(i = 0; i < p; i++)
		{
			GameObject obj = (GameObject)Instantiate(pine);
			obj.SetActive(false);
			pines.Add(obj);
		}
		for(i = 0; i < o; i++)
		{
			GameObject obj = (GameObject)Instantiate(oak);
			obj.SetActive(false);
			oaks.Add(obj);
		}
		for(i = 0; i < d; i++)
		{
			GameObject obj = (GameObject)Instantiate(dirt);
			obj.SetActive(false);
			dirts.Add(obj);
		}
		for(i = 0; i < s; i++)
		{
			GameObject obj = (GameObject)Instantiate(sand);
			obj.SetActive(false);
			sands.Add(obj);
		}
		for(i = 0; i < w; i++)
		{
			GameObject obj = (GameObject)Instantiate(water);
			obj.SetActive(false);
			waters.Add(obj);
		}
	}
	public GameObject GetMap ()
	{
		for (int i = 0; i < maps.Count; i++)
		{
			if(!maps[i].activeInHierarchy)
			{
				return maps[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (map);
		maps.Add (obj);
		return obj;
	}
	public GameObject GetRock()
	{
		for (int i = 0; i < rocks.Count; i++)
		{
			if(!rocks[i].activeInHierarchy)
			{
				return rocks[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (rock);
		rocks.Add (obj);
		return obj;
	}
	public GameObject GetPine()
	{
		for (int i = 0; i < pines.Count; i++)
		{
			if(!pines[i].activeInHierarchy)
			{
				return pines[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (pine);
		pines.Add (obj);
		return obj;
	}
	public GameObject GetOak()
	{
		for (int i = 0; i < oaks.Count; i++)
		{
			if(!oaks[i].activeInHierarchy)
			{
				return oaks[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (oak);
		oaks.Add (obj);
		return obj;
	}
	public GameObject GetDirt()
	{
		for (int i = 0; i < dirts.Count; i++)
		{
			if(!dirts[i].activeInHierarchy)
			{
				return dirts[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (dirt);
		dirts.Add (obj);
		return obj;
	}
	public GameObject GetSand()
	{
		for (int i = 0; i < sands.Count; i++)
		{
			if(!sands[i].activeInHierarchy)
			{
				return sands[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (sand);
		sands.Add (obj);
		return obj;
	}
	public GameObject GetWater()
	{
		for (int i = 0; i < waters.Count; i++)
		{
			if(!waters[i].activeInHierarchy)
			{
				return waters[i];
			}
		}
		GameObject obj = (GameObject)Instantiate (water);
		waters.Add (obj);
		return obj;
	}
	

}
