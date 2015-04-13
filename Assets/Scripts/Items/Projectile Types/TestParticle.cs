using UnityEngine;
using System.Collections;

public class TestParticle : BaseProjectile 
{

    public ParticleSystem particles;

	// Use this for initialization
	public override void Setup (GameObject shooter, float damageMod = 1.0f, float speedMod = 1.0f, int rotationMod = 0) 
    {
        base.Setup(shooter,damageMod,speedMod,rotationMod);
	    speed = 5; 
        lifespan = 2;
        particles.transform.position = this.transform.position;
        particles = (ParticleSystem)Instantiate(particles);
        //Debug.Break();
        particles.enableEmission = true;
        particles.Play();
	}
	
	// Update is called once per frame
    public override void Update() 
    {
        particles.transform.position = this.transform.position;
        base.Update();
	}
    public override void CleanUp()
    {
        particles.enableEmission = false;
        particles.Stop();
        //particles.Clear();
        particles.IsAlive(false);
        Destroy(particles.gameObject, 1.0f);
		base.CleanUp ();
    }
}
