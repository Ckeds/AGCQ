using UnityEngine;
using System.Collections;

public class DirtPile : BaseResource
{

		// Use this for initialization
		new public void Start ()
		{
			type = "dirt";
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
			base.TakeDamage (damageTaken);
			anim.SetFloat ("Health", currentHealth);
			//Debug.Log (currentHealth);
		}
}

