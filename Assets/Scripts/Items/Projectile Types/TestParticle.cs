using UnityEngine;
using System.Collections;

public class TestParticle : BaseProjectile 
{

    public ParticleSystem particles;
	public ParticleSystem contact;
	ParticleSystem p;
	ParticleSystem c;

	// Use this for initialization
	public override void Setup (GameObject shooter, float damageMod, float speedMod, float rotationMod) 
    {
		base.Setup(shooter,damageMod,speedMod,rotationMod);
	    speed = 5; 
        lifespan = 3;
		p = (ParticleSystem)Instantiate(particles);
        p.transform.position = this.transform.position;
        //Debug.Break();
        p.enableEmission = true;
        p.Play();
	}
	
	// Update is called once per frame
    public override void FixedUpdate() 
    {
		if(p != null)
        	p.transform.position = this.transform.position;
        base.FixedUpdate();
	}
    public override void CleanUp()
    {
		if(lifespan > 0)
		{
			c = (ParticleSystem)Instantiate(contact);
			c.transform.position = this.transform.position;
			if (this.GetComponent<Rigidbody2D>().velocity.y > 0)
				c.transform.position -= 3 * transform.forward;
			Destroy(c.gameObject, 1.0f);
		}
        p.enableEmission = false;
        p.Stop();
        //particles.Clear();
        p.IsAlive(false);
        Destroy(p.gameObject, 0.5f);
		base.CleanUp ();
    }
}
