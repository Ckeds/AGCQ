using UnityEngine;
using System.Collections;

public class SlimeMonster : Enemy 
{
    public override void Awake()
    {
        FindTarget();
        syncStartPosition = transform.position;
        syncEndPosition = transform.position;
        maxHealth = 15;
        currentHealth = maxHealth;
        isDamageable = true;
    }
	
	
	// Update is called once per frame
	new void Update () 
    {
        base.Update();
	}
}
