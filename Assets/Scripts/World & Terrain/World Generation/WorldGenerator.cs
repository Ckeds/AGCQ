using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour 
{
	[System.Serializable]
	public struct Resource
	{
		public Vector3 position;
		public char type;
	}

    public string mapSizeName = "Small";
    int mapSize;

    int meshSize = 16;
	public int mapUnitySize;
    int numRivers = 3;
    int numLakes = 2;
    int forestDensity = 5;
    int numDeserts = 2;
    int desertSize = 25;
    int desertDensity = 8000;
    int numStone = 2;
    int stoneSize = 35;
    int stoneDensity = 6000;
    int dirtPatches = 75;
    int forestThickness = 20000;

    int stonePercent = 15;
    int sandPercent = 15;
    int forestPercent = 25;
    int plainsPercent = 1;
    int dirtPercent = 1;
	int xMap;
	int yMap;
    bool walkOnWater = false;

    public Texture2D terrainTiles;
    public TDMap map;

    public GameObject TGmapPrefab;
	public Sprite[] mapTextures;
    public List<List<Resource>> resources;
	private int tileResolution = 64;
	Color[][] tileMap;
	public Vector3 [] tileMapLocations;
	public int[] textureAssignments;
	public List<int> mapsDrawn;
	public ResourceManager rM;
	int rocks = 0;
	int oaks = 0;
	int pines = 0;
	int sands = 0;
	int waters = 0;
	int dirts = 0;
	Camera c;
	bool coroutineDone;
	public bool drawAll = false;
    public bool generateResources = true;
	float time;

	// Use this for initialization
	void Start () 
    {
		time = Time.time;

        switch (mapSizeName)
        {
            case "Small":
                mapSize = 8;
                numRivers = 2;
                numLakes = 1;
                forestDensity = 1;
                dirtPatches = 50;
                numDeserts = 1;
                desertSize = 20;
                desertDensity = 5500;
                numStone = 1;
                stoneSize = 20;
                stoneDensity = 4000;
                forestThickness = 20000;
                break;

            case "Medium":
                mapSize = 12;
                numRivers = 4;
                numLakes = 2;
                forestDensity = 4;
                dirtPatches = 200;
                numDeserts = 1;
                desertSize = 40;
                desertDensity = 25000;
                numStone = 1;
                stoneSize = 45;
                stoneDensity = 8000;
                forestThickness = 20000;
                break;

            case "Large":
                mapSize = 16;
                numRivers = 8;
                numLakes = 4;
                forestDensity = 5;
                dirtPatches = 350;
                numDeserts = 2;
                desertSize = 40;
                desertDensity = 30000;
                numStone = 2;
                stoneSize = 45;
                stoneDensity = 10000;
                forestThickness = 30000;
                break;

            case "Huge":
                mapSize = 24;
                numRivers = 12;
                numLakes = 6;
                forestDensity = 7;
                dirtPatches = 500;
                numDeserts = 2;
                desertSize = 65;
                desertDensity = 80000;
                numStone = 2;
                stoneSize = 75;
                stoneDensity = 25000;
                forestThickness = 42000;
                break;

            default:
                mapSize = 12;
                numRivers = 4;
                numLakes = 2;
                forestDensity = 4;
                dirtPatches = 200;
                numDeserts = 1;
                desertSize = 40;
                desertDensity = 25000;
                numStone = 1;
                stoneSize = 45;
                stoneDensity = 8000;
                forestThickness = 20000;
                break;
        }
        if (!generateResources)
        {
            stonePercent = 0;
            sandPercent = 0;
            forestPercent = 0;
            plainsPercent = 0;
            dirtPercent = 0;
        }


		coroutineDone = false;
		mapUnitySize = mapSize * meshSize;
        StartCoroutine(buildWorld());

	}
    public IEnumerator buildWorld()
    {
		c = Camera.main;
		rM = GameObject.Find ("ResourcePool").GetComponent<ResourceManager> ();
		tileMapLocations = new Vector3[(mapSize + 2) * (mapSize + 2)];
		textureAssignments = new int[(mapSize + 2) * (mapSize + 2)];
		mapTextures = new Sprite[mapSize * mapSize];
		resources = new List<List<Resource>> ();
		mapsDrawn = new List<int> ();
        buildTDMap();
        Debug.Log(Time.time - time);
		tileMap = ChopUpTiles ();
		for (yMap = 0; yMap < mapSize; yMap++)
		{
			for (xMap = 0; xMap < mapSize; xMap++)
			{
				//Debug.Log("this, right?");
				BuildTexture(xMap * meshSize, yMap * meshSize);
				yield return null;
			}
		}
		rM.Setup (8, rocks / mapSize, pines / mapSize, oaks / mapSize, dirts / mapSize, sands / mapSize, waters / mapSize);
		buildTGMaps();
		coroutineDone = true;
		GameObject.Find("LoadBar").SetActive(false);
		GameObject.Find ("GameManagerGO").GetComponent<GUIManager> ().CurrentState = GUIManager.GUIState.StartScreen;
		map = null;
		time = Time.time - time;
		Debug.Log (time + " seconds");
		//Debug.Log (resources.Count);
		//CreateResource (resources [0]);
    }
    void buildTDMap()
    {
        map = new TDMap(meshSize*(mapSize), meshSize*(mapSize), numRivers, numLakes, dirtPatches, numDeserts, desertSize, desertDensity, numStone, stoneSize, stoneDensity,forestDensity,forestThickness);
    }
    void buildTGMaps()
    {
        //GameObject g = null;
        //Debug.Log(TGmap);
        //int numTiles = mapSize ^ 2;
        //Debug.Log(numTiles);
		//Debug.Log (tileMapLocations.Length);
        for (int y = 0; y < mapSize+2; y++)
        {
            for (int x = 0; x < mapSize+2; x++)
            {
                //Debug.Log("start inner loop");
				tileMapLocations[x + (y *(mapSize+2))] = new Vector3((x-1) * meshSize /* 0.64f*/, (y-1) * meshSize /* 0.64f*/, 1);
				if(x + (y *(mapSize+2)) == 0)
				{
					textureAssignments[x + (y *(mapSize+2))] = mapSize * mapSize - 1;
				}
				else if(x + (y *(mapSize+2)) < mapSize+2)
				{
					textureAssignments[x + (y *(mapSize+2))] = mapSize * mapSize - (mapSize - (x - 1));
					if(textureAssignments[x + (y *(mapSize+2))] >= mapSize * mapSize)
						textureAssignments[x + (y *(mapSize+2))] = mapSize * mapSize - mapSize;
				}
				else if(x == 0)
				{
					textureAssignments[x + (y *(mapSize+2))] = (mapSize * y) - 1;
					if(textureAssignments[x + (y *(mapSize+2))] >= mapSize * mapSize)
						textureAssignments[x + (y *(mapSize+2))] = mapSize - 1;					
				}
				else if(y > mapSize)
				{
					textureAssignments[x + (y *(mapSize+2))] = x - 1;
					if(textureAssignments[x + (y *(mapSize+2))] == mapSize)
						textureAssignments[x + (y *(mapSize+2))] = 0;
				}
				else if(x > mapSize)
				{
					textureAssignments[x + (y *(mapSize+2))] =  (y *mapSize) - mapSize;
				}
				else
				{
					textureAssignments[x + (y *(mapSize+2))] = (x + (y * mapSize) - (mapSize + 1));
				}
				if(drawAll)
				{
                //g = (GameObject)Instantiate(TGmapPrefab);
                // Debug.Log("calling Setup");
				//Debug.Log (tileMapLocations[x + (y *mapSize)]);
				//g.GetComponent<TGMap>().Setup(x*meshSize, y*meshSize, this.GetComponent<MeshRenderer>().sharedMaterials[(y*mapSize) + x], meshSize);
                //Debug.Log("End inner loop");
					mapsDrawn.Add(x + (y *(mapSize+2)));
					GameObject g = rM.GetMap();
					g.SetActive(true);
					//Debug.Log(g.activeInHierarchy);
					g.transform.position = tileMapLocations[x + (y *(mapSize+2))];
					//Debug.Log(textureAssignments[i]);
					g.GetComponent<TGMap>().Setup(mapTextures[textureAssignments[x + (y *(mapSize+2))]], resources[textureAssignments[x + (y *(mapSize+2))]], rM, false);
				}
            }
        }
    }
    public List<Resource> placeResource(int tileType, List<Resource> res, int x, int y)
    {
		int rand = Random.Range(1, 100);
        Resource r = new Resource();
		r.type = 'z';
        switch (tileType)
        {
            case 0:
				if (rand <= plainsPercent)
				{
				    int random = Random.Range(0, 3);
         	       if (random == 0)
            	    {
                  	 	//rock
						r.position = new Vector2(x + .5f, y + .5f);
						r.type = 'r';
						rocks++;
               		}
                	else if (random == 1)
                	{
						r.position = new Vector2(x + .5f, y + .5f);
						r.type = 'd';
						dirts++;
					}
				else if (random == 2)
                	{
                    	int randTreeA = Random.Range(0, 10);
                    	if (randTreeA == 0)
                    	{
							r.position = new Vector2(x + .5f, y + .5f);
							r.type = 'p';
							pines++;
						}
						else
						{
							r = new Resource();
							r.position = new Vector2(x + .5f, y + .5f);
							r.type = 'o';
							oaks++;
						}
					}
				}
				break;

			case 1:
				if(!walkOnWater)
				{
					r.position = new Vector2(x + .5f, y + .5f);
					r.type = 'w';
					waters++;
				}
			break;

            case 2:
				if ( rand <= forestPercent)
				{
              	  	int randTree = Random.Range(0, 10);
					if (randTree == 0)
					{
						r.position = new Vector2(x + .5f, y + .5f);
						r.type = 'p';
						pines++;
					}
					else
					{
						r.position = new Vector2(x + .5f, y + .5f);
						r.type = 'o';
						oaks++;
					}
				}
				break;

			case 3:
				if(rand <= sandPercent)
				{
					r.position = new Vector2(x + .5f, y + .5f);
					r.type = 's';
					sands++;
				}
			break;

            case 4:
				if(rand <= dirtPercent)
				{
					r.position = new Vector2(x + .5f, y + .5f);
					r.type = 'd';
					dirts++;
				}
				break;
			
		case 5:
				if(rand <= stonePercent)
				{
					r.position = new Vector2(x + .5f, y + .5f);
					r.type = 'r';
					rocks++;
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
		int texWidth = meshSize * tileResolution;
		int texHeight = meshSize * tileResolution;
		Texture2D mapTexture = new Texture2D(texWidth, texHeight);
		List<Resource> mapResources = new List<Resource>();
		//Debug.Log("Start texture loops");
		for (int y = startY; y < startY + meshSize; y++)
		{
			for (int x = startX; x < startX + meshSize; x++)
			{
				int tileType = map.GetTileAt(x, y);
				Color[] p = tileMap[tileType];
				mapTexture.SetPixels((int)((x-startX) * tileResolution), (int)((y-startY) * tileResolution), (int)(tileResolution), (int)(tileResolution), p);
				mapResources = placeResource(tileType, mapResources, x-startX, y-startY);
			}
		}
		//Debug.Log("End texture loops");
		
		mapTexture.filterMode = FilterMode.Trilinear;
		mapTexture.wrapMode = TextureWrapMode.Clamp;
		mapTexture.Apply();
		Sprite s = Sprite.Create(mapTexture, new Rect(0,0,meshSize * tileResolution, meshSize * tileResolution), new Vector2(0,0));
		resources.Add (mapResources);
		mapTextures [(startX / meshSize) + (startY * mapSize / meshSize)] = s;
		
	}

	public void Update()
	{
		if(coroutineDone)
		{
			float camX = c.transform.position.x;
			float camY = c.transform.position.y;
			float size = c.orthographicSize;
			for (int i = 0; i < tileMapLocations.Length; i++)
			{
				float dX = camX - (tileMapLocations[i].x + meshSize * 0.5f);
				float dY = camY - (tileMapLocations[i].y + meshSize * 0.5f);
				float distance = Mathf.Sqrt((dX * dX) + (dY * dY));
				if(mapsDrawn.Contains(i))
				{
					if(distance >= Mathf.Sqrt(meshSize * meshSize / 2) + 10 + size && !drawAll)
					{
						mapsDrawn.Remove(i);
					}
				}
				else
				{
					//Debug.Log (Mathf.Sqrt(meshSize * meshSize / 2));
					if(distance <= Mathf.Sqrt(meshSize * meshSize / 2) + 10 + size)
					{
						//Debug.Log ("Map " + i + " is ready to be drawn.");
						//Debug.Log((i + ": " + tileMapLocations[i].x) + ", " + (tileMapLocations[i].y ));
						mapsDrawn.Add(i);
						GameObject g = rM.GetMap();
						g.SetActive(true);
						//Debug.Log(g.activeInHierarchy);
						g.transform.position = tileMapLocations[i];
						//Debug.Log(textureAssignments[i]);
						g.GetComponent<TGMap>().Setup(mapTextures[textureAssignments[i]], resources[textureAssignments[i]], rM, true);
					}
				}
			}
		}
		else
		{
			GameObject.Find("LoadBar/Progress").transform.localScale = new Vector3((float)(yMap * mapSize + xMap) / (mapSize * mapSize),1,1);
			Debug.Log(GameObject.Find("LoadBar/Progress"));
		}
	}
}
