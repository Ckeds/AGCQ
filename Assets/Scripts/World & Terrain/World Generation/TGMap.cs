using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TGMap : MonoBehaviour
{
	public List<GameObject> resource;
	float width;
	float height;
	ResourceManager rM;
	Camera c;
	bool destroy;

    // Use this for initialization
	public void Setup(Sprite map, List<Resource> res, ResourceManager r, bool d)
    {
        //Debug.Log("Building a mesh...");
		c = Camera.main;
		resource = new List<GameObject> ();
		this.GetComponent<SpriteRenderer> ().sprite = map;
		rM = r;
		//Debug.Log ("HERE");
		width = map.rect.width * 1.5625f / 100;
		height = map.rect.height * 1.5625f / 100;
		this.transform.localScale = new Vector3 (1.5625f, 1.5625f, 1);
		destroy = d;
		CreateResource (res);
	}
    void Update()
    {	
		//Debug.Log ("here");
		float dX = c.transform.position.x - (this.transform.position.x + (width / 2));
		float dY = c.transform.position.y - (this.transform.position.y + (height / 2));
		float size = c.orthographicSize;
		float distance = Mathf.Sqrt((dX * dX) + (dY * dY));
		//Debug.Log (distance);
		if(distance >= Mathf.Sqrt(width * height / 2) + 5 + (size * 2) && destroy)
		{
			Deactivate();
		}
	}
	public void Deactivate()
	{
		foreach(GameObject g in resource)
		{
			g.SetActive(false);
		}
		this.gameObject.SetActive(false);
	}
	public void CreateResource(List<Resource> r)
	{
		GameObject g = null;
		Vector3 mapPos = this.transform.position;
		for (int i = 0; i < r.Count; i++)
		{
			switch(r[i].type)
			{
			case 'p': 
				g =  rM.GetPine();
				break;
			case 'o': 
				g =  rM.GetOak();
				break;
			case 's': 
				g =  rM.GetSand();
				break;
			case 'd': 
				g =  rM.GetDirt();
				break;
			case 'r': 
				g =  rM.GetRock();
				break;
			case 'w': 
				g =  rM.GetWater();
				break;
			}
			Vector3 p = r[i].position;
			g.transform.position = p + mapPos - (3 * transform.forward);
			g.SetActive(true);
			resource.Add(g);
		}
	}	
}
