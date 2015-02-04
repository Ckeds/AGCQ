using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour 
{
	public int numTilesX;
	public int numTilesY;
	public float tileSize = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		//A size of 1 doesnt redner
		if(numTilesX < 1)
		{
			numTilesX = 1;
		}
		if(numTilesY < 1)
		{
			numTilesY = 1;
		}
        numTilesX++;
        numTilesY++;
		
		BuildMesh ();
	}
	
	
	void BuildMesh () 
	{
		int numTiles = numTilesX * numTilesY;
		int numTriangles = numTiles * 2;
		int vSizeX = numTilesX + 1;
		int vSizeY = numTilesY + 1;
		int numVerts = vSizeX * vSizeY;
		
		//generate all the mesh data
		Vector3[] verticies = new Vector3 [numVerts];
		Vector3[] normals = new Vector3[numVerts];
		//Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[numTriangles * 3];
		
		int x, y;
		
		for(y=0; y < numTilesY; y++)
		{
			for(x=0; x < numTilesX; x++)
			{
				verticies[y * vSizeX + x] = new Vector3(x*tileSize,y*tileSize,0);
				normals [y* vSizeX + x] = Vector3.up;
				//uv [y * vSizeX + x] = new Vector2((float)x/vSizeX, (float)y/vSizeY);
			}
		}
		
		for(y=0; y < numTilesY; y++)
		{
			for(x=0; x < numTilesX; x++)
			{
				int squareIndex = y*numTilesX+x;
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
		//mesh.uv = uv;
		
		//assign mesh to filer/renderer/collider/etc
		MeshFilter mesh_filter = GetComponent < MeshFilter > ();
		MeshRenderer mesh_renderer = GetComponent < MeshRenderer > ();
		MeshCollider mesh_collider = GetComponent < MeshCollider > ();
		
		mesh_filter.mesh = mesh;
	}
}
