using UnityEngine;
using System.Collections;

public class SandPile : BaseResource
{

		// Use this for initialization
		new public void Awake ()
		{
			type = "sand";
			tier = 1;
			anim = GetComponent<Animator>();
			maxHealth = 10;
			currentHealth = 10;
			anim.SetFloat ("Health", currentHealth);
			base.Awake ();
		}

		public override void TakeDamage (int damageTaken)
		{
			base.TakeDamage (damageTaken);
			anim.SetFloat ("Health", currentHealth);
			//Debug.Log (currentHealth);
		}
}

