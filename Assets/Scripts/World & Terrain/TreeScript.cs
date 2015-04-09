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
	new public void Awake () 
	{
		type = "tree";
		tier = 1;
		anim = GetComponent<Animator>();
		startGrown = true;
		maxHealth = 10;
		currentHealth = 10;
		anim.SetFloat ("Time", growthTimer);
		base.Awake ();
	}
	public override void OnBecameVisible()
	{
		InvokeRepeating ("AnimValue", 0, 1f);
		base.OnBecameVisible ();
	}
	public override void OnBecameInvisible()
	{
		CancelInvoke ();
		base.OnBecameInvisible ();
	}
	void AnimValue () 
	{
		growthTimer = Time.time - Time.deltaTime;
		if (startGrown)
			growthTimer += 500;
		anim.SetFloat ("Time", growthTimer);
		//Debug.Log (growthTimer);
	}
}
