using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TGMap : MonoBehaviour
{
	Texture2D textureMap;

    // Use this for initialization
    public void Setup(Texture2D map, Rect r)
    {
        //Debug.Log("Building a mesh...");
        textureMap = map;
		this.GetComponent<SpriteRenderer> ().sprite = 
			Sprite.Create(textureMap, r, new Vector2(0,0));
		this.transform.localScale = new Vector3 (1.5625f, 1.5625f, 1);
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

}
