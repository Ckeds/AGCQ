using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour 
{
	public int sizeX;
	public int sizeY;
    int tileResolution = 10;
	public float tileSize = 1.0f;
	
	// Use this for initialization
	void Start () 
	{	
		BuildMesh ();
	}

    void BuildTexture()
    {
        Texture2D texture = new Texture2D(10, 10);

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Color c = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                texture.SetPixel(x, y, c);
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }
	
	public void BuildMesh () 
	{
		int numTiles = sizeX * sizeY;
		int numTriangles = numTiles * 2;
		int vSizeX = sizeX + 1;
		int vSizeY = sizeY + 1;
		int numVerts = vSizeX * vSizeY;
		
		//generate all the mesh data
		Vector3[] verticies = new Vector3 [numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[numTriangles * 3];
		
		int x, y;
		
		for(y=0; y < sizeY; y++)
		{
			for(x=0; x < sizeX; x++)
			{
				verticies[y * vSizeX + x] = new Vector3(x*tileSize,y*tileSize,1);
				normals [y* vSizeX + x] = Vector3.up;
				uv [y * vSizeX + x] = new Vector2((float)x/sizeX, (float)y/sizeY);
			}
		}
		
		for(y=0; y < sizeY; y++)
		{
			for(x=0; x < sizeX; x++)
			{
				int squareIndex = y*sizeX+x;
				int TriOffset = squareIndex*6;
				
				triangles[TriOffset + 0] = (y * vSizeX) + x;
				triangles[TriOffset + 1] = (y * vSizeX) + vSizeX + x;
                triangles[TriOffset + 2] = (y * vSizeX) + vSizeX + x + 1;
				
				triangles[TriOffset + 3] = (y * vSizeX) + x;
				triangles[TriOffset + 4] = (y * vSizeX) + vSizeX + x + 1;
                triangles[TriOffset + 5] = (y * vSizeX) + x + 1;
				
			}
		}
		
		//create the mesh and populate it with data
		Mesh mesh = new Mesh ();
		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		//assign mesh to filer/renderer/collider/etc
		MeshFilter mesh_filter = GetComponent < MeshFilter > ();
		MeshRenderer mesh_renderer = GetComponent < MeshRenderer > ();
		MeshCollider mesh_collider = GetComponent < MeshCollider > ();
		
		mesh_filter.mesh = mesh;
        mesh_collider.sharedMesh = mesh;

        BuildTexture();
	}
}
