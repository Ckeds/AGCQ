using UnityEngine;
using System.Collections;

public class DirtPile : BaseResource
{

		// Use this for initialization
		new public void Start ()
		{
			type = 'd';
			tier = 1;
			anim = GetComponent<Animator>();
			maxHealth = 10;
			currentHealth = 10;
			anim.SetFloat ("Health", currentHealth);
			base.Start ();
		}
	
		// Update is called once per frame
		new public void Update ()
		{
			anim.SetFloat ("Health", currentHealth);
			Debug.Log (currentHealth);
			base.Update ();
		}
}

