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

	}
	public override void Update ()
	{
		base.Update ();
	}

	public void Collect()
	{

	}

}
