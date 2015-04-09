using UnityEngine;
using System.Collections;

public class BaseResource : WorldObject {

	protected string type;
	protected int tier;
	protected Animator anim;

	// Use this for initialization
	public override void Awake () 
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
	public virtual void OnBecameVisible() 
	{
		anim.enabled = true;
		anim.SetFloat ("Health", currentHealth);
	}
	public virtual void OnBecameInvisible() 
	{
		anim.enabled = false;
	}
}
