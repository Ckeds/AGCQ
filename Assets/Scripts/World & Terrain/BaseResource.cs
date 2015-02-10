using UnityEngine;
using System.Collections;

public class BaseResource : WorldObject {

	protected char type;
	protected int tier;



	// Use this for initialization
	public override void Start () 
	{
		isDamageable = true;
	}

	public void Collect()
	{

	}

}
