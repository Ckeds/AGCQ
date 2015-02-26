using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TGMap : MonoBehaviour
{
    //public float tileSize = 1.0f;
    public int mapSize = 128;
    public int numRivers = 0;
    public int numLakes = 0;

    public int stonePercent;
    public int sandPercent;
    public int forestPercent;
    public int plainsPercent;
    public int dirtPercent;

    public Texture2D terrainTiles;
    public TDMap map;
    private int tileResolution = 64;

    public GameObject rockPile;
    public GameObject dirtPile;
    public GameObject sandPile;
	public GameObject tree1;
	public GameObject tree2;

    public List<GameObject> Resources;


    // Use this for initialization
    void Start()
    {
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


        BuildMesh();
    }

    Color[][] ChopUpTiles()
    {
        int numTilesPerRow = (int)(terrainTiles.width / tileResolution);
        int numRows = (int)(terrainTiles.height / tileResolution);

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }

        return tiles;
    }

    void BuildTexture()
    {
        map = new TDMap(mapSize, mapSize,numRivers,numLakes);

        int texWidth = mapSize * tileResolution;
        int texHeight = mapSize * tileResolution;
        Texture2D mapTexture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles();

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Color[] p = tiles[map.GetTileAt(x, y)];
                mapTexture.SetPixels((int)(x * tileResolution), (int)(y * tileResolution), (int)(tileResolution), (int)(tileResolution), p);
            }
        }

        mapTexture.filterMode = FilterMode.Trilinear;
        mapTexture.wrapMode = TextureWrapMode.Repeat;
        mapTexture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();

        mesh_renderer.sharedMaterials[0].mainTexture = mapTexture;

    }

     public void BuildMesh()
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] 
        {
         new Vector3(0, 0, 0),
         new Vector3(mapSize, 0, 0),
         new Vector3(mapSize, mapSize, 0),
         new Vector3(0, mapSize, 0)
       };
        m.uv = new Vector2[] 
        {
         new Vector2 (0, 0),
         new Vector2 (1, 0),
         new Vector2(1, 1),
         new Vector2 (0, 1)
       };
        m.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.Optimize();

        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = m;
        mesh_collider.sharedMesh = m;

        BuildTexture();
        placeResources();
    }

     public void placeResources()
     {
         Resources.Clear();
         foreach (TDTile tile in map.mapData)
         {
             if (tile.tileType == TDTypes.TYPE.OCEAN)
             {

             }
             else if (tile.tileType == TDTypes.TYPE.FOREST)
             {
                 int rand = Random.Range(1, 100);
                 if(rand <=forestPercent)
                 {
                     placeResource(tile);
                 }
             }
             else if (tile.tileType == TDTypes.TYPE.DESERT)
             {
                 int rand = Random.Range(1, 100);
                 if (rand <= sandPercent)
                 {
                     placeResource(tile);
                 }
             }
             else if (tile.tileType == TDTypes.TYPE.STONE)
             {
                 int rand = Random.Range(1, 100);
                 if (rand <= stonePercent)
                 {
                     placeResource(tile);
                 }
             }
             else if (tile.tileType == TDTypes.TYPE.DIRT)
             {
                 int rand = Random.Range(1, 100);
                 if (rand <= dirtPercent)
                 {
                     placeResource(tile);
                 }
             }
             else if (tile.tileType == TDTypes.TYPE.GRASS)
             {
                 int rand = Random.Range(1, 100);
                 if (rand <= plainsPercent)
                 {
                     placeResource(tile);
                 }
             }
         }
     }

    public void placeResource(TDTile tile)
    {
        GameObject g = null;
        switch (tile.tileType)
        {
            case TDTypes.TYPE.GRASS:
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    //rock
                    g = (GameObject) Instantiate(rockPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                }
                else if (rand == 1)
                {
                    g = (GameObject) Instantiate(dirtPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                }
                break;

            case TDTypes.TYPE.DESERT:
                g = (GameObject) Instantiate(sandPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                break;

            case TDTypes.TYPE.FOREST:
				int randTree = Random.Range(0, 10);
				if (randTree == 0)
				{
					//rock
					g = (GameObject) Instantiate(tree1, new Vector3(tile.positionX + 0.5f, tile.positionY, 0), Quaternion.identity);
				}
				else
				{
					g = (GameObject) Instantiate(tree2, new Vector3(tile.positionX + 0.5f, tile.positionY, 0), Quaternion.identity);
				}
                break;

            case TDTypes.TYPE.DIRT:
                g = (GameObject) Instantiate(dirtPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                break;

            case TDTypes.TYPE.STONE:
                g = (GameObject) Instantiate(rockPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);               
                break;

            default:
                break;
        }
        if(g != null)
            Resources.Add(g);
    }
    


}
