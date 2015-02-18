using UnityEngine;
using System.Collections;

public class Player : WorldObject
{
	//Player Defense Values
	//int physDefense;
	//int acidDefense;
	//int coldDefense;
	//int elecDefense;
	//int fireDefense;

	//Player Mana Values
	int currentMana;
	int maxMana;

	//Player Inventory
	// Player's inventory goes here

	// Use this for initialization
	void Start ()
	{
		//int physicalDefense = 0;
		//int acidDefense = 0;
		//int coldDefense = 0;
		//int electricDefense = 0;
		//int fireDefense = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			//check for item or weapon here
			
		}
	}

	//if the current item is not a weapon, and the player left clicks, use this
	void UseItem()
	{

	}

	//if the current item is a weapon, and the player left clicks, use this
	void UseWeapon()
	{

	}
}

