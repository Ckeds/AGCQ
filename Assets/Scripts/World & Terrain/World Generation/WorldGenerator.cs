using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

public class WorldGenerator : MonoBehaviour 
{
	[System.Serializable]
	public struct Resource
	{
		public Vector3 position;
		public char type;
	}

    public int mapSize;

    public int meshSize = 128;
    public int numRivers = 0;
    public int numLakes = 0;
    public int forestDensity = 5;
    public int numDeserts = 2;
    public int desertSize = 25;
    public int desertDensity = 8000;
    public int numStone = 2;
    public int stoneSize = 35;
    public int stoneDensity = 6000;

    public int stonePercent;
    public int sandPercent;
    public int forestPercent;
    public int plainsPercent;
    public int dirtPercent;
    public bool walkOnWater;

    public Texture2D terrainTiles;
    public TDMap map;

    public GameObject rockPile;
    public GameObject dirtPile;
    public GameObject sandPile;
    public GameObject tree1;
    public GameObject tree2;
    public GameObject waterCollider;

    public GameObject TGmapPrefab;
    public List<List<Resource>> Resources;
	private int tileResolution = 64;
	Color[][] tileMap;
	public Vector2 [] tileMapLocations;
	bool[] alreadyMade;
	// Use this for initialization
	void Start () 
    {
        if (mapSize < 1)
            mapSize = 1;
		tileMapLocations = new Vector2[(mapSize + 2) * (mapSize + 2)];
        if (stonePercent < 0)
            stonePercent = 0;
        if (stonePercent > 100)
            stonePercent = 100;

        if (sandPercent < 0)
            sandPercent = 0;
        if (sandPercent > 100)
            sandPercent = 100;

        if (forestPercent < 0)
            forestPercent = 0;
        if (forestPercent > 100)
            forestPercent = 100;

        if (plainsPercent < 0)
            plainsPercent = 0;
        if (plainsPercent > 100)
            plainsPercent = 100;

        if (dirtPercent < 0)
            dirtPercent = 0;
        if (dirtPercent > 100)
            dirtPercent = 100;
        buildWorld();



	}
    public void buildWorld()
    {
		Resources = new List<List<Resource>> ();
        buildTDMap();
		tileMap = ChopUpTiles ();
        buildTGMaps();
		CreateResource (Resources [0]);
		CreateResource (Resources [1]);
		CreateResource (Resources [4]);
		CreateResource (Resources [5]);
		Debug.Log (Resources [0].Count + Resources [1].Count + Resources [4].Count + Resources [5].Count);
    }
    void buildTDMap()
    {
        map = new TDMap(meshSize*(mapSize), meshSize*(mapSize), numRivers, numLakes, numDeserts, desertSize, desertDensity, numStone, stoneSize, stoneDensity,forestDensity);
    }
    void buildTGMaps()
    {
        //GameObject g = null;
        //Debug.Log(TGmap);
        //int numTiles = mapSize ^ 2;
        //Debug.Log(numTiles);
        for (int y = 0; y < mapSize+2; y++)
        {
            for (int x = 0; x < mapSize+2; x++)
            {
                //Debug.Log("start inner loop");
                //Debug.Log(this.GetComponent<MeshRenderer>().sharedMaterials[(y * mapSize) + x]);
				tileMapLocations[x + (y *(mapSize+2))] = new Vector2((x-1) * meshSize /* 0.64f*/, (y-1) * meshSize /* 0.64f*/);
                //g = (GameObject)Instantiate(TGmapPrefab);
                // Debug.Log("calling Setup");
				//Debug.Log (tileMapLocations[x + (y *mapSize)]);
				//g.GetComponent<TGMap>().Setup(x*meshSize, y*meshSize, this.GetComponent<MeshRenderer>().sharedMaterials[(y*mapSize) + x], meshSize);
                //Debug.Log("End inner loop");
            }
        }
		for (int yMap = 0; yMap < mapSize; yMap++)
		{
			for (int xMap = 0; xMap < mapSize; xMap++)
			{
				BuildTexture(xMap * meshSize, yMap * meshSize);
			}
		}
    }
    public List<Resource> placeResource(TDTile tile, List<Resource> res)
    {
		int rand = Random.Range(1, 100);
        Resource r = new Resource();
		r.type = 'z';
        switch (tile.tileType)
        {
            case TDTypes.TYPE.GRASS:
				if (rand <= plainsPercent)
				{
				    int random = Random.Range(0, 3);
         	       if (random == 0)
            	    {
                  	 	//rock
						r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
						r.type = 'r';
               		}
                	else if (random == 1)
                	{
						r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
						r.type = 'd';
					}
				else if (random == 2)
                	{
                    	int randTreeA = Random.Range(0, 10);
                    	if (randTreeA == 0)
                    	{
							r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
							r.type = 'p';
						}
						else
						{
							r = new Resource();
							r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
							r.type = 'o';
						}
					}
				}
				break;

            case TDTypes.TYPE.DESERT:
				if(rand <= sandPercent)
				{
					r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
					r.type = 's';
				}
			break;

            case TDTypes.TYPE.FOREST:
				if ( rand <= forestPercent)
				{
              	  	int randTree = Random.Range(0, 10);
					if (randTree == 0)
					{
						r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
						r.type = 'p';
					}
					else
					{
						r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
						r.type = 'o';
					}
				}
				break;

            case TDTypes.TYPE.DIRT:
				if(rand <= dirtPercent)
				{
					r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
					r.type = 'd';
				}
				break;
			
		case TDTypes.TYPE.STONE:
				if(rand <= stonePercent)
				{
					r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
					r.type = 'r';
				}
			break;
			
		case TDTypes.TYPE.OCEAN:
				if(!walkOnWater)
				{
					r.position = new Vector2(tile.positionX + .5f, tile.positionY + .5f);
					r.type = 'w';
				}
			break;
			
		default:
                break;
        }
		if (r.type != 'z')
			res.Add (r);
		return res;
    }
	Color[][] ChopUpTiles()
	{
		//Debug.Log("Startchopping");
		int numTilesPerRow = (int)(terrainTiles.width / tileResolution);
		int numRows = (int)(terrainTiles.height / tileResolution);
		
		Color[][] tiles = new Color[numTilesPerRow * numRows][];
		for (int y = 0; y < numRows; y++)
		{
			for (int x = 0; x < numTilesPerRow; x++)
			{
				tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels((x) * tileResolution, (y) * tileResolution, tileResolution, tileResolution);
			}
		}
		
		//Debug.Log("End chopping");
		return tiles;
	}
	
	void BuildTexture(int startX, int startY)
	{
		//Debug.Log("Start a Texture");
		int meshToUse = (startX / meshSize) + (startY * mapSize / meshSize);
		int texWidth = meshSize * tileResolution;
		int texHeight = meshSize * tileResolution;
		Texture2D mapTexture = new Texture2D(texWidth, texHeight);
		List<Resource> mapResources = new List<Resource>();
		//Debug.Log("Start texture loops");
		for (int y = startY; y < startY + meshSize; y++)
		{
			for (int x = startX; x < startX + meshSize; x++)
			{
				Color[] p = tileMap[map.GetTileAt(x, y)];
				mapTexture.SetPixels((int)((x-startX) * tileResolution), (int)((y-startY) * tileResolution), (int)(tileResolution), (int)(tileResolution), p);
				mapResources = placeResource(map.mapData[x,y], mapResources);
			}
		}
		//Debug.Log("End texture loops");
		
		mapTexture.filterMode = FilterMode.Trilinear;
		mapTexture.wrapMode = TextureWrapMode.Clamp;
		mapTexture.Apply();
		Resources.Add (mapResources);
		//Debug.Log (Resources[startX/meshSize + (startY/meshSize) * mapSize].Count);
		GetComponent<MeshRenderer> ().sharedMaterials [meshToUse].mainTexture = mapTexture;
		
	}
	public void Update()
	{
		for (int i = 0; i < tileMapLocations.Length; i++)
		{
			float dX = Camera.main.transform.position.x - (tileMapLocations[i].x + meshSize * 0.5f);
			float dY = Camera.main.transform.position.y - (tileMapLocations[i].y + meshSize * 0.5f);
			float distance = Mathf.Sqrt((dX * dX) + (dY * dY));
			//Debug.Log (Mathf.Sqrt(meshSize * meshSize / 2));
			if(distance <= Mathf.Sqrt(meshSize * meshSize / 2) + 10)
			{
				Debug.Log ("Map " + i + " is ready to be drawn.");
				//Debug.Log((i + ": " + tileMapLocations[i].x) + ", " + (tileMapLocations[i].y ));
			}
		}
	}
	public void CreateResource(List<Resource> r)
	{
		for (int i = 0; i < r.Count; i++)
			{
			switch(r[i].type)
			{
				case 'p': 
					Instantiate(tree1, r[i].position, Quaternion.identity);
					break;
				case 'o': 
					Instantiate(tree2, r[i].position, Quaternion.identity);
					break;
				case 's': 
					Instantiate(sandPile, r[i].position, Quaternion.identity);
					break;
				case 'd': 
					Instantiate(dirtPile, r[i].position, Quaternion.identity);
					break;
				case 'r': 
					Instantiate(rockPile, r[i].position, Quaternion.identity);
					break;
				case 'w': 
					Instantiate(waterCollider, r[i].position, Quaternion.identity);
					break;
			}
		}
	}
}
