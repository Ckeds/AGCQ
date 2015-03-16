using UnityEngine;
using System.Collections;

public class BaseEquipment : BaseItem {

	protected float physDamage;

	// Use this for initialization
	new public void Start () 
	{
		type = "equipment";
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void Update () 
	{
		
	}
	
}
