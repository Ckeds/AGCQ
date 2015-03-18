using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class BaseProjectile : MonoBehaviour 
{
    public float speed = 5;
    public float lifespan;
    public int damage;

	// Use this for initialization
	public virtual void Awake () 
    {
	
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {
        lifespan -= Time.deltaTime;
        if(lifespan<=0)
        { Destroy(this.gameObject); }
        else
        {
            this.transform.position = new Vector3(rigidbody2D.position.x, rigidbody2D.position.y, 0);
            //Debug.Log(rigidbody2D.velocity);
        }
	}
    public void Setup(GameObject shooter)
    {
        this.transform.position = shooter.transform.position;
        this.transform.rotation = shooter.transform.rotation;
        Debug.Log(this.transform.up * speed);
        //rigidbody2D.AddForce(this.transform.up * speed);
        rigidbody2D.velocity = this.transform.up * speed;
        rigidbody2D.velocity += shooter.GetComponent<Rigidbody2D>().velocity;
    }
}
