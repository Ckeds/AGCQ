﻿using UnityEngine;
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
		//Debug.Log (currentHealth);
		if (currentHealth <= 0)
					OnDeath ();
	}

	public virtual void Move(float XMove, float YMove)
	{
		positionX += XMove;
		positionY += YMove;
	}
	public virtual void TakeDamage(int damageTaken)
	{
		//Debug.Log ("I SHOULD DO THIS");
		//Debug.Log (currentHealth);
		if (isDamageable)
			currentHealth -= damageTaken;
	}
	public virtual void OnDeath()
	{
		Destroy (this.gameObject);
		Network.Destroy(this.gameObject);
	}



}
