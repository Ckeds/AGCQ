using UnityEngine;
using System.Collections;

public class RockPile : BaseResource
{

	// Use this for initialization
	new public void Start ()
	{
		//Debug.Log ("I DID THIS TOO");
		type = "rock";
		tier = 1;
		anim = GetComponent<Animator>();
		maxHealth = 10;
		currentHealth = 10;
		anim.SetFloat ("Health", currentHealth);
		base.Start ();
	}
		
	// Update is called once per frame
	public override void TakeDamage (int damageTaken)
	{
		Debug.Log ("DOING WORK");
		base.TakeDamage (damageTaken);
		anim.SetFloat ("Health", currentHealth);
		Debug.Log (currentHealth);
	}
}

