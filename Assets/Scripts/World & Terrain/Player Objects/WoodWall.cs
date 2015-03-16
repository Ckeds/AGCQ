using UnityEngine;
using System.Collections;

public class WoodWall : BaseWall 
{

	// Use this for initialization
	new void Start () 
    {
        maxHealth = 150;
        currentHealth = 150;
        armor = 2;
	}
}
