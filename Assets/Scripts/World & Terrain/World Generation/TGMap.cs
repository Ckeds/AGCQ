using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TGMap : MonoBehaviour
{
	public GameObject rockPile;
	public GameObject dirtPile;
	public GameObject sandPile;
	public GameObject tree1;
	public GameObject tree2;
	public GameObject waterCollider;
	
	public List<GameObject> resource;
	float width;
	float height;

    // Use this for initialization
	public void Setup(Sprite map, List<WorldGenerator.Resource> res)
    {
        //Debug.Log("Building a mesh...");
		this.GetComponent<SpriteRenderer> ().sprite = map;
		width = map.rect.width * 1.5625f / 100;
		height = map.rect.height * 1.5625f / 100;
		this.transform.localScale = new Vector3 (1.5625f, 1.5625f, 1);
		CreateResource (res);
	}
    void Update()
    {	
		//Debug.Log ("here");
		float dX = Camera.main.transform.position.x - (this.transform.position.x + (width / 2));
		float dY = Camera.main.transform.position.y - (this.transform.position.y + (height / 2));
		float size = Camera.main.orthographicSize;
		float distance = Mathf.Sqrt((dX * dX) + (dY * dY));
		//Debug.Log (distance);
		if(distance >= Mathf.Sqrt(width * height / 2) + 5 + (size * 2))
		{
			foreach(GameObject g in resource)
			{
				Destroy(g);
			}
			Destroy(this.gameObject);
		}
	}
	public void CreateResource(List<WorldGenerator.Resource> r)
	{
		GameObject g = null;
		Vector3 mapPos = this.transform.position;
		for (int i = 0; i < r.Count; i++)
		{
			switch(r[i].type)
			{
			case 'p': 
				g =  (GameObject)Instantiate(tree1, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			case 'o': 
				g = (GameObject)Instantiate(tree2, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			case 's': 
				g = (GameObject)Instantiate(sandPile, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			case 'd': 
				g = (GameObject)Instantiate(dirtPile, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			case 'r': 
				g = (GameObject)Instantiate(rockPile, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			case 'w': 
				g = (GameObject)Instantiate(waterCollider, r[i].position + mapPos - (3 * transform.forward), Quaternion.identity);
				break;
			}
			resource.Add(g);
		}
	}
	
}
