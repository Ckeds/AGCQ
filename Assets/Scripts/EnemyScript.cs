using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public GameObject explosionPrefab;
	
	private MeshRenderer attractFlag;
	
	public float counter = 0;
	public float moveSpeed = 0;
	private float findInterval = .25f;
	private bool attracted = false;
	public bool Attracted 
	{
		get { return attracted; }
	}
	private bool stunned = false;
	
	public float speed = 5;
	public float Speed
	{
		get
		{
			if (stunned)
				return 0;
			else if (!Attracted)
				return speed;
			else
				return speed * .25f;
		}
		set { speed = value; }
	}
	public float accel;
	public float Accel
	{
		get
		{
			if (stunned)
				return 0;
			else if (!Attracted)
				return accel; 
			else
				return accel * 2f;
		}
		set { accel = value; }
	}
	
	public Vector3 target;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private float syncStartRotation = 0f;
	private float syncEndRotation = 0f;
	private float syncTime = 0f;
	private float lastSyncTime = 0f;
	private float syncDelay = 0f;
	
	// Use this for initialization
	void Start () 
	{
		FindTarget();
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		//attractFlag = GetComponentsInChildren<MeshRenderer>()[1];
		//attractFlag.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		counter += Time.deltaTime;
		
		if (networkView.isMine)
		{
			Vector2 previousForce = rigidbody2D.velocity;
			if (counter > 0)
			{
				FindTarget();
				//Debug.Log(target);
				counter = -findInterval;
				//attractFlag.enabled = false;
				attracted = false;
				stunned = false;
			}
			
			Vector2 delta = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
			
			//rigidbody.MovePosition(Vector3.Lerp(transform.position, target, 7));
			//Debug.Log(delta);
			delta.Normalize();
			delta *= Accel;
			Accel = 2 + rigidbody2D.velocity.magnitude/4;
			rigidbody2D.AddForce(-previousForce);
			rigidbody2D.AddForce(delta * 1.5f);
			if (rigidbody2D.velocity.magnitude > Speed)
			{
				Debug.Log("SLOW DOWN");
				Vector2 slow = -rigidbody2D.velocity;
				slow.Normalize();
				slow *= rigidbody2D.velocity.magnitude - Speed;
				
				rigidbody2D.AddForce(slow);
			}
			float angle = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
			angle = angle * (180 / Mathf.PI) - 90; 
			//Debug.Log (angle);
			rigidbody2D.rotation = angle;
			moveSpeed = rigidbody2D.velocity.magnitude;
			
		}
		else
		{
			SyncedMovement();
			if (counter > 0)
			{
				counter = -findInterval;
				attractFlag.enabled = false;
				attracted = false;
				stunned = false;
			}
		}
		
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody2D.position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
		rigidbody2D.rotation = Mathf.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{	
		Vector3 networkPosition = Vector3.zero;
		Vector3 networkVelocity = Vector3.zero;
		float networkRotation = 0f;
		float networkAngVelocity = 0f;
		
		if (stream.isWriting)
		{
			networkPosition = rigidbody2D.position;
			networkVelocity = rigidbody2D.velocity;
			networkRotation = rigidbody2D.rotation;
			networkAngVelocity = rigidbody2D.angularVelocity;

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
			lastSyncTime = Time.time;
			
			syncStartPosition = rigidbody2D.position;
			syncEndPosition = networkPosition + networkVelocity * syncDelay;
			syncStartRotation = rigidbody2D.rotation;
			syncEndRotation = networkRotation;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		//coll.gameObject.GetComponent<CombatScript> ().AlterHealth (1);
	}
	
	private void FindTarget()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length > 0)
			target = players[Random.Range(0, players.Length)].transform.position;
		else
		{
			Explode();
			GameObject.Find("GameManagerGO").GetComponent<SpawnManager>().SpawnEnemies = false;
		}
	}
	
	public void AttractTo(Vector3 point)
	{
		target = point;
		networkView.RPC("AttractedTo", RPCMode.All, networkView.viewID);
	}
	
	[RPC]
	public void AttractedTo(NetworkViewID id)
	{
		if (networkView.viewID == id)
		{
			counter = -findInterval;
			attractFlag.enabled = true;
			attracted = true;
		}
	}
	
	public void Explode()
	{
		//networkView.RPC("Exploded", RPCMode.All, transform.position, transform.rotation); 
		GetComponent<CombatScript>().Die();
	}
	
	[RPC]
	public void Exploded(Vector3 pos, Quaternion rot)
	{
		GameObject.Instantiate(explosionPrefab, pos, rot);
	}
	
	public void Stun(float time)
	{
		networkView.RPC("Stunned", RPCMode.All, networkView.viewID, time);
	}
	
	[RPC]
	public void Stunned(NetworkViewID id, float time)
	{
		if (networkView.viewID == id)
		{
			counter = -time;
			attractFlag.enabled = true;
			stunned = true;
		}
	}
}
