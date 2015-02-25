using UnityEngine;
using System.Collections;

public class TreeScript : BaseResource {

	bool startGrown = false;
	float growthTimer = 0;


	public TreeScript(bool isGrown)
	{
		startGrown = isGrown;
	}

	// Use this for initialization
	new public void Start () 
	{
		type = "tree";
		tier = 1;
		anim = GetComponent<Animator>();
		maxHealth = 10;
		currentHealth = 10;
		anim.SetFloat ("Time", growthTimer);
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		growthTimer = Time.time - Time.deltaTime;
		anim.SetFloat ("Time", growthTimer);
		base.Update ();
		//Debug.Log (growthTimer);
	}
}
