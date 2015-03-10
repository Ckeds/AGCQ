using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TGMap : MonoBehaviour
{

    public Texture2D terrainTiles;
    public TDMap map;
    private int tileResolution = 64;
    int meshSize = 128;
    int startX;
    int startY;
    Material textureMap;


    // Use this for initialization
    public void Setup(TDMap mapIn, int xPos, int yPos, Material m)
    {
        
        map = mapIn;
        startX = xPos;
        startY = yPos;
        //Debug.Log("Building a mesh...");
        textureMap = m;
        BuildMesh();
        this.transform.position = new Vector3(startX / (100 * 64), startY / (100 * 64), 1);
    }
    void Start()
    {
        
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

    void BuildTexture(TDMap mapInput)
    {
        //Debug.Log("Start a Texture");
        map = mapInput;

        int texWidth = 128 * tileResolution;
        int texHeight = 128 * tileResolution;
        Texture2D mapTexture = new Texture2D(texWidth, texHeight);
        
        Color[][] tiles = ChopUpTiles();

        //Debug.Log("Start texture loops");
        for (int y = startY; y < startY + 128; y++)
        {
            for (int x = startX; x < startX + 128; x++)
            {
                Color[] p = tiles[map.GetTileAt(x, y)];
                mapTexture.SetPixels((int)((x-startX) * tileResolution), (int)((y-startY) * tileResolution), (int)(tileResolution), (int)(tileResolution), p);
                
            }
        }
        //Debug.Log("End texture loops");

        mapTexture.filterMode = FilterMode.Trilinear;
        mapTexture.wrapMode = TextureWrapMode.Repeat;
        mapTexture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        Material[] mat = mesh_renderer.sharedMaterials;
        mat[0] = textureMap;
        Debug.Log(mat[0]);
        mesh_renderer.sharedMaterials = mat;
        Debug.Log(mesh_renderer.sharedMaterials[0]);
        mesh_renderer.sharedMaterials[0].mainTexture = mapTexture;
        //Debug.Log("End a texture");

    }

     public void BuildMesh()
    {
        //Debug.Log("Start a mesh");
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] 
        {
         new Vector3(startX, startY, 0),
         new Vector3(startX+128, startY, 0),
         new Vector3(startX+128, startY+128, 0),
         new Vector3(startX, startY+128, 0)
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

        //Debug.Log("End a mesh");
        BuildTexture(map);
    }

}
