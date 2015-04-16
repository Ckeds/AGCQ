using UnityEngine;
using System.Collections;

public class TestParticle : BaseProjectile 
{

    public ParticleSystem particles;
	public ParticleSystem contact;

	// Use this for initialization
	public override void Setup (GameObject shooter, float damageMod = 1.0f, float speedMod = 1.0f, int rotationMod = 0) 
    {
	    speed = 5; 
        lifespan = 3;
        particles.transform.position = this.transform.position;
        particles = (ParticleSystem)Instantiate(particles);
        //Debug.Break();
        particles.enableEmission = true;
        particles.Play();
		base.Setup(shooter,damageMod,speedMod,rotationMod);
	}
	
	// Update is called once per frame
    public override void FixedUpdate() 
    {
        particles.transform.position = this.transform.position;
        base.FixedUpdate();
	}
    public override void CleanUp()
    {
		if(lifespan > 0)
		{
			contact.transform.position = this.transform.position;
			if (this.GetComponent<Rigidbody2D>().velocity.y > 0)
				contact.transform.position -= 3 * transform.forward;
			contact = (ParticleSystem)Instantiate(contact);
			Destroy(contact.gameObject, 1.0f);
		}
        particles.enableEmission = false;
        particles.Stop();
        //particles.Clear();
        particles.IsAlive(false);
        Destroy(particles.gameObject, 0.5f);
		base.CleanUp ();
    }
}
