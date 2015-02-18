using UnityEngine;
using System.Collections;

public class BaseResource : WorldObject {

	protected string type;
	protected int tier;
	protected Animator anim;


	// Use this for initialization
	protected void Start () 
	{
		isDamageable = true;

	}
	
	// Update is called once per frame
	protected void Update () 
	{
		base.Update ();
	}
	/*
	protected void Move ()
	{

	}

	//handle this object taking damage
	protected void TakeDamage(int damageTaken)
	{
		if (isDamageable)
			currentHealth -= damageTaken;
	}

	//handle the resource death
	protected void OnDeath()
	{
		Destroy(this, 0.0F); //float is amount of time to wait until destroying the object
	}*/

	public void Collect()
	{

	}

}
