using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : WorldObject
{
	//Name of the player
	GameObject playerName;
	public GameObject PlayerName
	{
		get { return playerName;}
		set {playerName = value;}
	}

	//Player
	public GameObject player;
	public GameObject itemFab;
    public GameObject testParticle;
	Camera c;
	List<GameObject> bullets;
	Rigidbody2D rigid;

	//Animator
	Animator animator;

	//Movement variables
	public Vector2 movement;
	float v = 0f;
	float h = 0f;
	float previousV = 0f;
	float previousH = 0f;
	float scale = 1f;
	float scaleSprint = 1.5f;
	float maxX;
	float maxY;
	public Vector3 mouse;

	//Sync varibles
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;

	//Network movement variables
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	Vector2 syncVelocity;
	Quaternion syncRotation;

	//Player Defense Values
	//int physicalDefense;
	//int acidDefense;
	//int coldDefense;
	//int electricDefense;
	//int fireDefense;

	//Player Mana Values
	int currentMana;
	int maxMana;

	//Player Inventory
	// Player's inventory goes here
	public GameObject[] items;
	GameObject[] equipped;

	// Use this for initialization

	public override void Awake()
	{
		maxHealth = 50;
		currentHealth = maxHealth;
		//physicalDefense = 0;
		//acidDefense = 0;
		//coldDefense = 0;
		//electricDefense = 0;
		//fireDefense = 0;
		isDamageable = false;
		animator = GetComponent<Animator> ();
		bullets = new List<GameObject> ();
		if (!GetComponent<NetworkView>().isMine)
		{
			animator.applyRootMotion = false;
		}
		else
		{
			c = Camera.main;
		}
		items [0] = (GameObject)Instantiate (itemFab);
		items [0].hideFlags = HideFlags.HideInHierarchy;
		//Destroy (items [0]);
		for (int i = 0; i < items.Length; i++)
		{
			if(items[i] != null)
			{
				items [i].GetComponent<BaseItem>().colorToDraw = Random.Range(0,5);
				Debug.Log(items [i].GetComponent<BaseItem>().colorToDraw);
				items [i].GetComponent<BaseItem>().CreateGUITex ();
				items [i].GetComponent<BaseItem>().CreateGUITexHover ();
			}
		}
		maxX = GameObject.Find ("GameManagerGO").GetComponent<WorldGenerator> ().MapUnitySize;
		maxY = maxX;
		syncStartPosition = transform.position;
		syncRotation = Quaternion.identity;
		lastSyncTime = Time.time;
		rigid = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	public void Update ()
	{
        GameObject g = null;
		if (Input.GetMouseButtonDown (0) && GetComponent<NetworkView>().isMine) 
		{
			//check for item or weapon here
			g = GetBullet();
			g.SetActive(true);
			g.GetComponent<BaseProjectile>().Setup(this.gameObject, 0, 0, 0);            
		}
		//Debug.Log (1 / Time.deltaTime);
	}
	public GameObject GetBullet()
	{
		for (int i = 0; i < bullets.Count; i++)
		{
			if(!bullets[i].activeInHierarchy)
			{
				return bullets[i];
			}
		}
		if (bullets.Count < 55)
		{
			//GameObject obj = (GameObject)Instantiate(testParticle);
			GameObject obj = (GameObject)Network.Instantiate (testParticle, new Vector3(-60, -60), Quaternion.identity, 3);
			bullets.Add (obj);
			return obj;
		}
		return null;
	}
	public void FixedUpdate()
	{
		Move();
	}
	void Move()
	{
		if (GetComponent<NetworkView>().isMine) 
			InputMovement ();
		else 
			SyncedMovement();
		Vector2 p = rigid.position;
		if(p.x > maxX)
		{
			p.x -= maxX;
		}
		if(this.transform.position.y > maxY)
		{
			p.y -= maxY;
		}
		if(this.transform.position.x < 0)
		{
			p.x += maxX;
		}
		if(this.transform.position.y < 0)
		{
			p.y += maxY;
		}
		rigid.position = p;
		this.transform.position = new Vector3 (rigid.position.x, rigid.position.y, -1);
	}
	private void InputMovement()
	{
		Vector2 previousForce = rigid.velocity;
		//Debug.Log (previousForce);

		previousV = v;
		
		previousH = h;

		//used to get input for direction
		v = Input.GetAxis("Vertical");
		h = Input.GetAxis("Horizontal");
		//Check for sprint
		if(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))
		{
			v *= scaleSprint;
			h *= scaleSprint;
		}
		else
		{
			v *= scale;
			h *= scale;
		}
		
		//handle going the opposite direction
		if (v * previousV < 0)
		{
			v += (previousV * 29);
			v = v / 30;
			previousForce.y *= 10f;
		}
		if (h * previousH < 0)
		{
			h += (previousH * 29);
			h = h / 30;
			previousForce.x *= 10f;
		}
		//store Movement
		movement = new Vector2 (h, v);

		//Debug.Log (movement);
		//following code used to make player character face mouse
		//mouse = c.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;       //Mouse position
		//mouse.Normalize ();
		//float rotation = Mathf.Atan2 (mouse.y, mouse.x) * Mathf.Rad2Deg;
		float angle = Mathf.Atan2(GetComponent<ConstantForce2D>().force.y, GetComponent<ConstantForce2D>().force.x) * (180 / Mathf.PI) - 90;
		float mod = angle % 45;
		if (mod >= 22.5)
			angle = angle - mod + 45;
		else
			angle = angle - mod;
		//Debug.Log (angle);
		//Debug.Log (rigid.angularVelocity);

		//mouse.z = this.transform.position.z;
		if (GetComponent<ConstantForce2D> ().force.magnitude > .75)
		{
			rigid.rotation = 0;
			rigid.angularVelocity = 0;
			this.transform.rotation = Quaternion.identity;
			this.transform.rotation = Quaternion.Euler (0f, 0f, angle);
		}
		
		//Uncomment this block to make the player move based on the mouse cursor
		/*float mouseDist = Mathf.Sqrt((relmousepos.x * relmousepos.x) + (relmousepos.y * relmousepos.y)) * 10
		if (mouseDist > 0.7f || v >= 0) 
		{
			movement = angle * movement;
		}
		*/
		//apply

		//this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		float charSpeed = movement.sqrMagnitude;
	
		animator.SetFloat ("charSpeed", charSpeed);
		//Vector2 newPosition = rigidbody2D.position + (movement * Time.deltaTime);
		//Debug.Log (newPosition);
		//rigidbody2D.position = newPosition;
		GetComponent<ConstantForce2D>().force =  (movement * 100);
	}
	private void SyncedMovement ()
	{
		syncTime += Time.deltaTime;
		Debug.Log ("SyncStart : " + syncStartPosition);
		//Debug.Log ("SyncEnd : " + syncEndPosition);
		this.transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		rigid.position = this.transform.position;
		this.transform.rotation = syncRotation;
		//Debug.Log (syncRotation);
		//Debug.Log (this.transform.position);
		rigid.rotation = 0;
		rigid.angularVelocity = 0;
		rigid.velocity = Vector2.zero;
		float charSpeed = syncVelocity.sqrMagnitude;
		this.GetComponent<ConstantForce2D> ().force = syncVelocity;
		animator.SetFloat ("charSpeed", charSpeed);
	}
	//if the current item is not a weapon, and the player left clicks, use this
	void UseItem()
	{

	}

	//if the current item is a weapon, and the player left clicks, use this
	void UseWeapon()
	{

	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{	
		Vector3 networkPosition = Vector3.zero;
		Vector3 networkVelocity = Vector3.zero;
		Quaternion networkRotation = Quaternion.identity;
		
		if (stream.isWriting)
		{
			networkPosition = this.transform.position;
			networkVelocity = rigid.velocity;
			networkRotation = this.transform.rotation;
			
			
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);
			stream.Serialize(ref networkRotation);
			
		}
		else
		{	
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);
			stream.Serialize(ref networkRotation);
			
			Debug.Log (networkPosition);
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;
			syncStartPosition = networkPosition;
			syncEndPosition = syncStartPosition + (networkVelocity * syncDelay);
			syncVelocity = networkVelocity;
			//Debug.Log("syncEndPosition : " + syncEndPosition);
			syncRotation = this.transform.rotation;
			GetComponent<ConstantForce2D>().force = networkVelocity;
		}
	}
	public override void OnDeath ()
	{
		Network.Destroy (playerName);
		base.OnDeath ();
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Resource" && coll.gameObject.GetComponent<WorldObject> ())
			coll.gameObject.GetComponent<WorldObject> ().TakeDamage (1);
	}
	[RPC]
	void SetupPlayer(NetworkViewID id)
	{
		player = NetworkView.Find(id).gameObject;
	}
}

