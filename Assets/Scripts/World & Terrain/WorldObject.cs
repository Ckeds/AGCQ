using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour 
{
	protected float positionX, positionY;
	protected int currentHealth;
	protected int maxHealth;
	protected bool isDamageable;

	// Use this for initialization
	public void Start () 
	{
		isDamageable = false;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if (currentHealth <= 0)
					OnDeath ();
	}

	void Move(float XMove, float YMove)
	{
		positionX += XMove;
		positionY += YMove;
	}
	public void TakeDamage(int damageTaken)
	{
		Debug.Log ("I SHOULD DO THIS");
		Debug.Log (currentHealth);
		if (isDamageable)
						currentHealth -= damageTaken;
	}
	void OnDeath()
	{
		Destroy (this.gameObject, 0.0f);
		Destroy(this, 0.0F); //float is amount of time to wait until destroying the object
	}


}
