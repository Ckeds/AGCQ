using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TGMap : MonoBehaviour
{
    //public float tileSize = 1.0f;
    public int mapSize = 128;

    public Texture2D terrainTiles;
    int tileResolution = 64;

    // Use this for initialization
    void Start()
    {
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
        TDMap map = new TDMap(mapSize, mapSize);

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
    }
    


}
