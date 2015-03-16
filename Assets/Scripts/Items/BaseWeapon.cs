using UnityEngine;
using System.Collections;

public class BaseWeapon : BaseItem {

	protected float physDamage;

	// Use this for initialization
	new public void Start () 
	{
		type = "weapon";
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void Update () 
	{
	
	}
}
