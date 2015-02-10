using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour 
{
	protected float positionX, positionY;
	protected int currentHealth;
	protected int maxHealth;
	protected bool isDamageable;

	// Use this for initialization
	public virtual void Start () 
	{
		isDamageable = false;
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		if (currentHealth <= 0)
					OnDeath ();
		Debug.Log (currentHealth);
	}

	public virtual void Move(float XMove, float YMove)
	{
		positionX += XMove;
		positionY += YMove;
	}
	public virtual void TakeDamage(int damageTaken)
	{
		if (isDamageable)
						currentHealth -= damageTaken;
	}
	public virtual void OnDeath()
	{
		Network.Destroy(this.gameObject); //float is amount of time to wait until destroying the object
	}



}
