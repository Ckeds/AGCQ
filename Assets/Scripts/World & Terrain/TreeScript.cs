using UnityEngine;
using System.Collections;

public class TreeScript : BaseResource {

	public bool startGrown = true;
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
		startGrown = true;
		maxHealth = 10;
		currentHealth = 10;
		anim.SetFloat ("Time", growthTimer);
		base.Start ();
	}
	
	// Update is called once per frame
	public void Update () 
	{
		growthTimer = Time.time - Time.deltaTime;
		if (startGrown)
			growthTimer += 500;
		anim.SetFloat ("Time", growthTimer);
		//Debug.Log (growthTimer);
	}
}
