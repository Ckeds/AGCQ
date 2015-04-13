using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class BaseProjectile : MonoBehaviour 
{
    public float speed;
    public float lifespan;
    public float damage;

	// Use this for initialization
	public virtual void Awake () 
    {
	
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {
        lifespan -= Time.deltaTime;
        if(lifespan<=0)
        {
            CleanUp(); 
        }
        else
        {
            this.transform.position = new Vector3(rigidbody2D.position.x, rigidbody2D.position.y, 0);
            //Debug.Log(rigidbody2D.velocity);
        }
	}
    public virtual void Setup(GameObject shooter, float damageMod = 1.0f, float speedMod = 1.0f, int rotationMod = 0)
    {
        damage *= damageMod;
        speed *= speedMod;

        this.transform.position = shooter.transform.position + transform.forward;
        this.transform.rotation = shooter.transform.rotation;
        this.transform.Rotate(0, 0, rotationMod);
        //rigidbody2D.AddForce(this.transform.up * speed);
        rigidbody2D.velocity = this.transform.up * speed;
        rigidbody2D.velocity += shooter.GetComponent<Rigidbody2D>().velocity;
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
		Destroy(this.gameObject);
    }
}
