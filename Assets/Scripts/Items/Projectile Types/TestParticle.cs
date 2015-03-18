using UnityEngine;
using System.Collections;

public class TestParticle : BaseProjectile 
{

	// Use this for initialization
	public override void Awake () 
    {
	    speed = 15;
        lifespan = 5;
        damage = 2;
	}
	
	// Update is called once per frame
    public override void Update() 
    {
        base.Update();
	}
}
