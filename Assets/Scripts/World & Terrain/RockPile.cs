using UnityEngine;
using System.Collections;

public class RockPile : BaseResource
{

		// Use this for initialization
		new void Start ()
		{
			type = 'r';
			tier = 1;
		}
	
		// Update is called once per frame
		new void Update ()
		{
			base.Update ();
		}
}

