using UnityEngine;
using System.Collections;

public class BaseResource : WorldObject {

	protected string type;
	protected int tier;
	protected Animator anim;

	// Use this for initialization
	public override void Start () 
	{
		isDamageable = true;
		//Debug.Log ("I did it");
		anim.enabled = false;
	}
	public override void TakeDamage (int damageTaken)
	{
		base.TakeDamage (damageTaken);
	}
	public void Collect()
	{

	}
	void OnBecameVisible() 
	{
		anim.enabled = true;
	}
	void OnBecameInvisible() 
	{
		anim.enabled = false;
	}
}
