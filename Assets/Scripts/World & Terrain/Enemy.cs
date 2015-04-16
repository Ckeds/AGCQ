using UnityEngine;
using System.Collections;

public class Enemy : WorldObject {

	//Movement and tracking variables
	public Vector3 target;
	public float counter = 0;
	public float moveSpeed = 0;
	private float findInterval = 0.5f;
	public float speed = 4;
	public float accel = 2;

	//animator
	Animator anim;

	//Sync varibles
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;
	
	//Network movement variables
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	float syncStartRotation = 0f;
	float syncEndRotation = 0f;


	// Use this for initialization
	public override void Awake()
	{
		FindTarget ();
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		maxHealth = 1;
		currentHealth = maxHealth;
		isDamageable = true;
		anim = this.gameObject.GetComponent<Animator> ();
		if (!GetComponent<NetworkView>().isMine)
			anim.applyRootMotion = false;
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		lastSyncTime = Time.time;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		counter += Time.deltaTime;
		Move ();
	}
	void Move()
	{
		if (GetComponent<NetworkView>().isMine) 
			AIMovement ();
		else 
			SyncedMovement();
	}
	private void AIMovement()
	{
		//Vector2 previousForce = GetComponent<Rigidbody2D>().velocity;
		if (counter > 0)
		{
			FindTarget();
			counter = -findInterval;
		}
		
		Vector2 delta = new Vector2(target.x - transform.position.x, target.y - transform.position.y);

		delta.Normalize();
		delta *= accel;
		accel = 2 + GetComponent<Rigidbody2D>().velocity.magnitude/4;
		//GetComponent<Rigidbody2D>().AddForce(-previousForce);
		GetComponent<Rigidbody2D>().AddForce(delta * 50);
		if (GetComponent<Rigidbody2D>().velocity.magnitude > speed)
		{
			//Debug.Log("SLOW DOWN");
			Vector2 slow = -GetComponent<Rigidbody2D>().velocity;
			slow.Normalize();
			slow *= GetComponent<Rigidbody2D>().velocity.magnitude - speed;
			
			GetComponent<Rigidbody2D>().AddForce(slow);
		}
		float angle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * (180 / Mathf.PI) - 90;
		GetComponent<Rigidbody2D>().rotation = angle;
		moveSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
		anim.SetFloat ("charSpeed", GetComponent<Rigidbody2D>().velocity.magnitude);
		this.transform.position = new Vector3(GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y, 0);
	}
	private void SyncedMovement()
	{
		//Debug.Log ("SyncStart : " + syncStartPosition);
		//Debug.Log ("SyncEnd : " + syncEndPosition);
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody2D>().position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
		GetComponent<Rigidbody2D>().rotation = Mathf.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
		this.transform.position = new Vector3(GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y, 0);
		//Debug.Log (rigidbody2D.velocity.sqrMagnitude);
		anim.SetFloat ("charSpeed", GetComponent<Rigidbody2D>().velocity.sqrMagnitude);
		if (counter > 0)
			counter = -findInterval;
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{	
		Vector3 networkPosition = Vector3.zero;
		Vector3 networkVelocity = Vector3.zero;
		float networkRotation = 0f;
		float networkAngVelocity = 0f;
		
		if (stream.isWriting)
		{
			networkPosition = GetComponent<Rigidbody2D>().position;
			networkVelocity = GetComponent<Rigidbody2D>().velocity;
			networkRotation = GetComponent<Rigidbody2D>().rotation;
			networkAngVelocity = GetComponent<Rigidbody2D>().angularVelocity;
			
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);
			stream.Serialize(ref networkRotation);
			stream.Serialize(ref networkAngVelocity);
		}
		else
		{
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);
			stream.Serialize(ref networkRotation);
			stream.Serialize(ref networkAngVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			//Debug.Log(syncDelay);
			lastSyncTime = Time.time;
			
			syncStartPosition = GetComponent<Rigidbody2D>().position;
			syncEndPosition = networkPosition + networkVelocity * syncDelay;
			syncStartRotation = GetComponent<Rigidbody2D>().rotation;
			syncEndRotation = networkRotation;
			GetComponent<Rigidbody2D>().velocity = networkVelocity;
		}
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.GetComponent<Player>())
			coll.gameObject.GetComponent<WorldObject> ().TakeDamage (1);
	}
	
	private void FindTarget()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		target = new Vector3 (50000, 50000, 50);
		if (players.Length > 0)
		{
			for (int i  = 0; i < players.Length; i++) 
			{
				Vector3 distance = target - this.transform.position;
				Vector3 tentative = players[i].transform.position - this.transform.position;
				if(tentative.magnitude < distance.magnitude)
					target = players[i].transform.position;
			}
		}
		else
		{
			OnDeath();
			GameObject.Find("GameManagerGO").GetComponent<SpawnManager>().SpawnEnemies = false;
		}
	}
}
