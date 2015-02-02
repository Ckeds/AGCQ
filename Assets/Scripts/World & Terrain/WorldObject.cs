using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour 
{
	float positionX, positionY;
	int currentHealth;
	int maxHealth;
	bool isDamageable;

	// Use this for initialization
	void Start () 
	{
		isDamageable = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currentHealth <= 0)
						OnDeath ();
	}

	void Move(float XMove, float YMove)
	{
		positionX += XMove;
		positionY += YMove;
	}
	void TakeDamage(int damageTaken)
	{
		if (isDamageable)
						currentHealth -= damageTaken;
	}
	void OnDeath()
	{
		Destroy(this, 0.0F); //float is amount of time to wait until destroying the object
	}


}
