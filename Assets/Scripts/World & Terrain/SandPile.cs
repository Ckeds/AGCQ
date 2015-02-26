using UnityEngine;
using System.Collections;

public class SandPile : BaseResource
{

		// Use this for initialization
		new public void Start ()
		{
			type = "sand";
			tier = 1;
			anim = GetComponent<Animator>();
			maxHealth = 10;
			currentHealth = 10;
			anim.SetFloat ("Health", currentHealth);
			base.Start ();

		}
	
		// Update is called once per frame
		public override void Update ()
		{
			anim.SetFloat ("Health", currentHealth);
			//Debug.Log (currentHealth);
			base.Update ();
		}
}

