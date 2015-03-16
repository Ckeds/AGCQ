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

	public virtual void Move(float XMove, float YMove)
	{
		positionX += XMove;
		positionY += YMove;
	}
	public virtual void TakeDamage(int damageTaken)
	{
		Debug.Log ("I SHOULD DO THIS");
		//Debug.Log (currentHealth);
		if (isDamageable)
			currentHealth -= damageTaken;
		if (currentHealth <= 0)
			OnDeath ();
	}
	public virtual void OnDeath()
	{
		if(this.GetComponent<NetworkView>())
		{
			Network.Destroy (this.gameObject);
		}
		Destroy(this.gameObject);
	}



}
