using UnityEngine;
using System.Collections;

public class WoodPile : BaseResource
{

		// Use this for initialization
		new public void Start ()
		{
			type = "wood";
			tier = 1;
		}
	
		// Update is called once per frame
		public override void Update ()
		{
			base.Update ();
		}
		
}

