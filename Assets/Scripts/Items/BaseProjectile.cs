using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class BaseProjectile : MonoBehaviour 
{
    public float speed;
    public float lifespan;
    public float damage;
	public Vector2 vel;
	float life;
	// Use this for initialization
	public virtual void Awake () 
    {
	
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate () 
    {
        lifespan -= Time.deltaTime;
        if(lifespan<=0)
        {
            CleanUp(); 
        }
        else
        {
			GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + (vel * Time.deltaTime));
            this.transform.position = new Vector3(GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y, 0);
			float alph = 3*lifespan/2*life;
			if(alph > 1)
				alph = 1;
			GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,alph);
            //Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }
	}
    public virtual void Setup(GameObject shooter, float damageMod, float speedMod, float rotationMod)
    {
        if(damageMod > 0)
        damage *= damageMod;
        if (speedMod > 0)
        speed *= speedMod;
		life = lifespan;
        this.transform.position = shooter.transform.position + transform.forward;
        this.transform.rotation = shooter.transform.rotation;
        this.transform.Rotate(0, 0, rotationMod);
        //rigidbody2D.AddForce(this.transform.up * speed);
        vel = this.transform.up * speed;
        vel += shooter.GetComponent<ConstantForce2D>().force / 100;
		//Debug.Log (vel);
    }
	public virtual void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.GetComponent<Enemy>())
		{
			coll.gameObject.GetComponent<WorldObject> ().TakeDamage (1);
			CleanUp();
		}
		if(coll.gameObject.GetComponent<BaseResource>())
			CleanUp();
	}
    public virtual void CleanUp()
    {
		this.gameObject.SetActive(false);
    }
}
