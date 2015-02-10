using UnityEngine;
using System.Collections;

public class RockPile : BaseResource
{

		// Use this for initialization
		void Start ()
		{
			type = 'r';
			tier = 1;
			anim = GetComponent<Animator>();
			maxHealth = 10;
			currentHealth = 10;
			anim.SetFloat ("Health", currentHealth);
	}
		
		// Update is called once per frame
		void Update ()
		{
			//anim.SetFloat ("Health", currentHealth);
			print (currentHealth);
			base.Update ();
		}

}

