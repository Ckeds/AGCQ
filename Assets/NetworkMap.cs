using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMap : MonoBehaviour {

	public Sprite[] mapTextures;
	public List<List<Resource>> resources;
	public Vector3 [] tileMapLocations;
	public int[] textureAssignments;
	public List<int> mapsDrawn;
	List<bool> giveADarn;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
