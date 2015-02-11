using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TGMap : MonoBehaviour
{

    public int sizeX = 100;
    public int sizeY = 50;
    public float tileSize = 1.0f;

    public int mapSize;

    public Texture2D terrainTiles;
    public int tileResolution;

    // Use this for initialization
    void Start()
    {
        BuildMesh();
    }

    Color[][] ChopUpTiles()
    {
        int numTilesPerRow = terrainTiles.width / tileResolution;
        int numRows = terrainTiles.height / tileResolution;

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
        TDMap map = new TDMap(sizeX, sizeY);

        int texWidth = sizeX * tileResolution;
        int texHeight = sizeY * tileResolution;
        Texture2D mapTexture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles();

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Color[] p = tiles[map.GetTileAt(x, y)];
                mapTexture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
            }
        }

        mapTexture.filterMode = FilterMode.Trilinear;
        mapTexture.wrapMode = TextureWrapMode.Clamp;
        mapTexture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();

        mesh_renderer.sharedMaterials[0].mainTexture = mapTexture;

    }

     public void BuildMesh()
    {
        int numTiles = sizeX * sizeY;
        int numTris = numTiles * 2;

        int vSizeX = sizeX + 1;
        int vSizeY = sizeY + 1;
        int numVerts = vSizeX * vSizeY;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];

        int x, y;
        for (y = 0; y < vSizeY; y++)
        {
            for (x = 0; x < vSizeX; x++)
            {
                vertices[y * vSizeX + x] = new Vector3(x * tileSize, y * tileSize, 0);
                normals[y * vSizeX + x] = Vector3.up;
                uv[y * vSizeX + x] = new Vector2((float)x / sizeX, (float)y / sizeY);
            }
        }

        for (y = 0; y < sizeY; y++)
        {
            for (x = 0; x < sizeX; x++)
            {
                int squareIndex = y * sizeX + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = y * vSizeX + x + 0;
                triangles[triOffset + 1] = y * vSizeX + x + vSizeX + 0;
                triangles[triOffset + 2] = y * vSizeX + x + vSizeX + 1;

                triangles[triOffset + 3] = y * vSizeX + x + 0;
                triangles[triOffset + 4] = y * vSizeX + x + vSizeX + 1;
                triangles[triOffset + 5] = y * vSizeX + x + 1;
            }
        }

        // Create a new Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
        mesh_collider.sharedMesh = mesh;

        BuildTexture();
    }
    


}
