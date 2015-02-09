using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
	public Vector2 movement;
	public Animator animator;
	//private int currentColliderIndex = 0;
    float v = 0;
    float h = 0;
	float previousV = 0;
	float previousH = 0;
    float scale = 4f;
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;
    float scaleSprint = 6f;
	Quaternion rotateValue;
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	float syncStartRotation = 0f;
	float syncEndRotation = 0f;
	GameObject player;
	public GameObject nameObject;
	[SerializeField]
	private PolygonCollider2D[] colliders;

	// Use this for initialization
	void Start () {
		if (!networkView.isMine)
		{
			animator.applyRootMotion = false;
		}
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (networkView.isMine) {
			InputMovement ();
		}
		else {
			SyncedMovement();
		}
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
        if(Input.GetKey(KeyCode.LeftShift))
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
			}
		}
		animator.SetFloat ("charSpeed", charSpeed);
		//Vector2 newPosition = rigidbody2D.position + (movement * Time.deltaTime);
		//Debug.Log (newPosition);
		//rigidbody2D.position = newPosition;
		rigidbody2D.AddForce (movement);
		this.transform.position = new Vector3 (rigidbody2D.position.x, rigidbody2D.position.y, 0);
		//Debug.Log (rigidbody2D.velocity);
    }
	private void SyncedMovement ()
	{
		syncTime += Time.deltaTime;
		Debug.Log ("SyncStart : " + syncStartPosition);
		Debug.Log ("SyncEnd : " + syncEndPosition);
		rigidbody2D.position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
		rigidbody2D.rotation = Mathf.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
		float charSpeed = rigidbody2D.velocity.sqrMagnitude;
		animator.SetFloat ("charSpeed", charSpeed);
		this.transform.position = new Vector3(rigidbody2D.position.x, rigidbody2D.position.y, 0);
	}
	void OnTriggerEnter2D( Collider2D other )
	{
		Debug.Log ("Hit " + other.gameObject);
	}
	void OnCollisionEnter2D( Collision2D coll )
	{
		Debug.Log ("Hit " + coll.gameObject);
	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{	
		Debug.Log ("Working!");
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
			Debug.Log ("I'm there");
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);
			stream.Serialize(ref networkRotation);
			stream.Serialize(ref networkAngVelocity);

			Debug.Log (networkPosition);
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;
			syncStartPosition = rigidbody2D.position;
			syncEndPosition = networkPosition + networkVelocity * syncDelay;
			syncStartRotation = rigidbody2D.rotation;
			syncEndRotation = networkRotation + networkAngVelocity * syncDelay;
			rigidbody2D.velocity = networkVelocity;
		}
	}
	[RPC]
	void SetupPlayer(NetworkViewID id)
	{
		player = NetworkView.Find(id).gameObject;
		Debug.Log (player);
		networkView.observed = this;
		
		//player.GetComponent<HumanScript>().clothes = player.GetComponentsInChildren<SkinnedMeshRenderer>()[2].materials[1];
		//Color color = Color.black;

	}
}
