using UnityEngine;
using System.Collections;

public class BaseItem : MonoBehaviour {

	protected string type;
	protected int tier;
	public int colorToDraw;
	public Color[] colors;
	protected Animator anim;
	protected Sprite itemSprite;
	protected string flavorText;
	public Texture2D guiTex;
	public Texture2D guiTexHover;
	public Texture2D GuiTex
	{
		get{ return guiTex;}
	}
	public Texture2D GuiTexHover
	{
		get{ return guiTexHover;}
	}
	public Texture2D getThatSprite()
	{
		return GetComponent<SpriteRenderer> ().sprite.texture;
	}
	// Use this for initialization
	protected void Start () 
	{

	}
	
	// Update is called once per frame
	protected void Update () 
	{
		CreateGUITex ();
	}

	public void CreateGUITex()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		Debug.Log (sr);
		Texture2D tex = sr.sprite.texture;
		Color[] spriteToDraw = tex.GetPixels();
		Debug.Log(spriteToDraw[0]);
		for(int i = 0; i < spriteToDraw.Length; i++)
		{
			if(spriteToDraw[i].a <= .2f)
			{
				spriteToDraw[i] = colors[colorToDraw];
				spriteToDraw[i].a = 0.3f;
			}
			else
			{
				spriteToDraw[i].a = .85f;
			}
		}
		Debug.Log(spriteToDraw[0]);
		guiTex =  new Texture2D(64,64);
		guiTex.SetPixels(spriteToDraw);
		guiTex.Apply(false, false);
	}
	public void CreateGUITexHover()
	{
		SpriteRenderer sr =GetComponent<SpriteRenderer>();
		Debug.Log (sr);
		Texture2D tex = sr.sprite.texture;
		Color[] spriteToDraw = tex.GetPixels();
		Debug.Log(spriteToDraw[0]);
		for(int i = 0; i < spriteToDraw.Length; i++)
		{
			if(spriteToDraw[i].a <= .2f)
			{
				spriteToDraw[i] = colors[colorToDraw];
				spriteToDraw[i].a = 0.4f;
			}
			else
			{
				spriteToDraw[i].a = .9f;
			}
		}
		Debug.Log(spriteToDraw[0]);
		guiTexHover =  new Texture2D(64,64);
		guiTexHover.SetPixels(spriteToDraw);
		guiTexHover.Apply(false, false);
	}
}
