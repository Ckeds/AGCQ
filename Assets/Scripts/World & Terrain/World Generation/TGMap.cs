using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TGMap : MonoBehaviour
{

    public Texture2D terrainTiles;
    public TDMap map;
    int meshSize = 128;
    Material textureMap;


    // Use this for initialization
    public void Setup(Material m, int mesh)
    {
        //Debug.Log("Building a mesh...");
        textureMap = m;
		meshSize = mesh;
        BuildMesh();
	}
    void Start()
    {
        
    }


    /*Color[][] ChopUpTiles()
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

    void BuildTexture(TDMap mapInput)
    {
        //Debug.Log("Start a Texture");
        map = mapInput;

        int texWidth = meshSize * tileResolution;
		int texHeight = meshSize * tileResolution;
        Texture2D mapTexture = new Texture2D(texWidth, texHeight);
        
        Color[][] tiles = ChopUpTiles();

        //Debug.Log("Start texture loops");
		for (int y = startY; y < startY + meshSize; y++)
        {
			for (int x = startX; x < startX + meshSize; x++)
			{
                Color[] p = tiles[map.GetTileAt(x, y)];
                mapTexture.SetPixels((int)((x-startX) * tileResolution), (int)((y-startY) * tileResolution), (int)(tileResolution), (int)(tileResolution), p);
                
            }
        }
        //Debug.Log("End texture loops");

        mapTexture.filterMode = FilterMode.Trilinear;
        mapTexture.wrapMode = TextureWrapMode.Clamp;
        mapTexture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        Material[] mat = mesh_renderer.sharedMaterials;
        mat[0] = textureMap;
        //Debug.Log(mat[0]);
        mesh_renderer.sharedMaterials = mat;
        //Debug.Log(mesh_renderer.sharedMaterials[0]);
        mesh_renderer.sharedMaterials[0].mainTexture = mapTexture;
        //Debug.Log("End a texture");

    }*/

     public void BuildMesh()
    {
        //Debug.Log("Start a mesh");
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] 
        {
         new Vector3(0, 0, 0),
         new Vector3(meshSize, 0, 0),
         new Vector3(meshSize, meshSize, 0),
         new Vector3(0, meshSize, 0)
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

        mesh_filter.mesh = m;
		GetComponent<MeshRenderer>().sharedMaterial = textureMap;
        //Debug.Log("End a mesh");
        //BuildTexture(map);
    }

}
