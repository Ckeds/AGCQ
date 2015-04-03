using UnityEngine;
using System.Collections;

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

	//Animator
	Animator animator;

	//Movement variables
	public Vector2 movement;
	float v = 0f;
	float h = 0f;
	float previousV = 0f;
	float previousH = 0f;
	float scale = 2f;
	float scaleSprint = 4f;

	//Sync varibles
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;

	//Network movement variables
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	float syncStartRotation = 0f;
	float syncEndRotation = 0f;

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
	public override void Start ()
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
		if (!networkView.isMine)
		{
			animator.applyRootMotion = false;
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
	}

	void Awake()
	{
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		lastSyncTime = Time.time;
	}

	// Update is called once per frame
	public void Update ()
	{
        GameObject g = null;
		if (Input.GetMouseButtonDown (0)) 
		{
			//check for item or weapon here
            g = (GameObject)Instantiate(testParticle);
            g.GetComponent<BaseProjectile>().Setup(this.gameObject, 0, 1, 0);

            g = (GameObject)Instantiate(testParticle);
            g.GetComponent<BaseProjectile>().Setup(this.gameObject, 0, 1, 5);

            g = (GameObject)Instantiate(testParticle);
            g.GetComponent<BaseProjectile>().Setup(this.gameObject, 0, 1, -5);
		}
		//Debug.Log (1 / Time.deltaTime);
		Move();
	}
	void Move()
	{
		if (networkView.isMine) 
			InputMovement ();
		else 
			SyncedMovement();
	}
	private void InputMovement()
	{
		Vector2 previousForce = rigidbody2D.velocity;
		if (v != 0) {
			previousV = v;
		}
		if (h != 0) {
			previousH = h;
		}
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
		
		//following code used to make player character face mouse
		Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);       //Mouse position
		Vector3 objpos = Camera.main.WorldToViewportPoint(transform.position);        //Object position on screen
		Vector2 relobjpos = new Vector2(objpos.x - 0.5f, objpos.y - 0.5f);            //Set coordinates relative to object's center
		Vector2 relmousepos = new Vector2(mouse.x - 0.5f, mouse.y - 0.5f) - relobjpos;//Mouse cursor relative to object's center
		float angle = Vector2.Angle(Vector2.up, relmousepos);                         //Angle calculation
		
		//if mouse is on the left side of our object
		if (relmousepos.x > 0)
			angle = 360 - angle;
		
		//Uncomment this block to make the player move based on the mouse cursor
		/*float mouseDist = Mathf.Sqrt((relmousepos.x * relmousepos.x) + (relmousepos.y * relmousepos.y)) * 10
		if (mouseDist > 0.7f || v >= 0) 
		{
			movement = angle * movement;
		}
		*/
		
		//apply
		rigidbody2D.rotation = angle;
		rigidbody2D.AddForce (-previousForce);
		float charSpeed = rigidbody2D.velocity.sqrMagnitude;
		if(v == 0 && h == 0)
		{
			rigidbody2D.AddForce (-previousForce * 2);
			if (charSpeed <= .1f) {
				rigidbody2D.velocity = new Vector2(0,0);
				previousH = 0;
				previousV = 0;
			}
		}
		animator.SetFloat ("charSpeed", charSpeed);
		//Vector2 newPosition = rigidbody2D.position + (movement * Time.deltaTime);
		//Debug.Log (newPosition);
		//rigidbody2D.position = newPosition;
		rigidbody2D.AddForce (movement);
		this.transform.position = new Vector3 (rigidbody2D.position.x, rigidbody2D.position.y, -1);
		//Debug.Log (rigidbody2D.velocity);
	}
	private void SyncedMovement ()
	{
		syncTime += Time.deltaTime;
		//Debug.Log ("SyncStart : " + syncStartPosition);
		//Debug.Log ("SyncEnd : " + syncEndPosition);
		rigidbody2D.position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
		rigidbody2D.rotation = Mathf.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
		float charSpeed = rigidbody2D.velocity.sqrMagnitude;
		animator.SetFloat ("charSpeed", charSpeed);
		this.transform.position = new Vector3(rigidbody2D.position.x, rigidbody2D.position.y, -1);
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
			
			//Debug.Log (networkPosition);
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;
			syncStartPosition = rigidbody2D.position;
			syncEndPosition = networkPosition + networkVelocity * syncDelay;
			//Debug.Log("syncEndPosition : " + syncEndPosition);
			syncStartRotation = rigidbody2D.rotation;
			syncEndRotation = networkRotation + networkAngVelocity * syncDelay;
			rigidbody2D.velocity = networkVelocity;
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

