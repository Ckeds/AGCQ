﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

public class WorldGenerator : MonoBehaviour 
{
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
    private int tileResolution = 64;

    public GameObject rockPile;
    public GameObject dirtPile;
    public GameObject sandPile;
    public GameObject tree1;
    public GameObject tree2;
    public GameObject waterCollider;

    public GameObject TGmapPrefab;

    public List<GameObject> Resources;

	// Use this for initialization
	void Start () 
    {
        if (mapSize < 1)
            mapSize = 1;

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

        //buildWorld();



	}
    public void buildWorld()
    {
        buildTDMap();
        buildTGMaps();
        placeResources();
    }
    void buildTDMap()
    {
        map = new TDMap(meshSize*(mapSize), meshSize*(mapSize), numRivers, numLakes, numDeserts, desertSize, desertDensity, numStone, stoneSize, stoneDensity,forestDensity);
    }
    void buildTGMaps()
    {
        GameObject g = null;
        //Debug.Log(TGmap);
        //int numTiles = mapSize ^ 2;
        //Debug.Log(numTiles);
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                //Debug.Log("start inner loop");
                Debug.Log(this.GetComponent<MeshRenderer>().sharedMaterials[(y * mapSize) + x]);
                g = (GameObject)Instantiate(TGmapPrefab);
               // Debug.Log("calling Setup");
                g.GetComponent<TGMap>().Setup(map, x*128, y*128, this.GetComponent<MeshRenderer>().sharedMaterials[(y*mapSize) + x]);
                //Debug.Log("End inner loop");
            }
        }
    }

    public void placeResources()
    {
        Resources.Clear();
        foreach (TDTile tile in map.mapData)
        {
            if (tile.tileType == TDTypes.TYPE.OCEAN)
            {
                if (!walkOnWater)
                {
                    placeResource(tile);
                }
            }
            else if (tile.tileType == TDTypes.TYPE.FOREST)
            {
                int rand = Random.Range(1, 100);
                if (rand <= forestPercent)
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
                    g = (GameObject)Instantiate(rockPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                }
                else if (rand == 1)
                {
                    g = (GameObject)Instantiate(dirtPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                }
                break;

            case TDTypes.TYPE.DESERT:
                g = (GameObject)Instantiate(sandPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                break;

            case TDTypes.TYPE.FOREST:
                int randTree = Random.Range(0, 10);
                if (randTree == 0)
                {
                    //rock
                    g = (GameObject)Instantiate(tree1, new Vector3(tile.positionX + 0.5f, tile.positionY, 0), Quaternion.identity);
                }
                else
                {
                    g = (GameObject)Instantiate(tree2, new Vector3(tile.positionX + 0.5f, tile.positionY, 0), Quaternion.identity);
                }
                break;

            case TDTypes.TYPE.DIRT:
                g = (GameObject)Instantiate(dirtPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                break;

            case TDTypes.TYPE.STONE:
                g = (GameObject)Instantiate(rockPile, new Vector3(tile.positionX, tile.positionY + 1, 0), Quaternion.identity);
                break;

            case TDTypes.TYPE.OCEAN:
                g = (GameObject)Instantiate(waterCollider, new Vector3(tile.positionX + .5f, tile.positionY + .5f, 0), Quaternion.identity);
                break;

            default:
                break;
        }
        if (g != null)
            Resources.Add(g);
    }
}