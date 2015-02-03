using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
	public Vector3 movement;
	public Animator animator;
	//private int currentColliderIndex = 0;
    float v = 0;
    float h = 0;
	float previousV = 0;
	float previousH = 0;
    float scale = 0.05f;
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;
    float scaleSprint = 0.075f;
	Quaternion rotateValue;
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	float syncStartRotation = 0f;
	float syncEndRotation = 0f;
	GameObject player;
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
	void Update () {
		if (networkView.isMine) {
			InputMovement ();
		}
		else {
			SyncedMovement();
		}
	}
    private void InputMovement()
    {
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
		}
		if (h * previousH < 0)
		{
			h += (previousH * 29);
			h = h / 30;
		}
        //store Movement
        movement = new Vector3(h, v, 0);
		float charSpeed = Mathf.Sqrt ((v * v) + (h * h)) * 100;
		animator.SetFloat ("charSpeed", charSpeed);

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
			rotateValue = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		//movement = rotateValue * movement;
		*/

		//Comment if using previous block
		rotateValue = Quaternion.AngleAxis(angle, Vector3.forward);

		//apply
        transform.rotation = rotateValue;
        transform.position += movement;
    }
	private void SyncedMovement ()
	{
		syncTime += Time.deltaTime;
		rigidbody2D.position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
		rigidbody2D.rotation = Mathf.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
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
			syncEndRotation = networkRotation + networkAngVelocity * syncDelay;
			
			v = Vector3.Dot(syncStartPosition - syncEndPosition, -transform.forward) / syncDelay;
			h = Vector3.Dot(syncStartPosition - syncEndPosition, -transform.right) / syncDelay;
		}
	}
	[RPC]
	void SetupPlayer(NetworkViewID id)
	{
		player = NetworkView.Find(id).gameObject;
		Debug.Log (player);
		
		//player.GetComponent<HumanScript>().clothes = player.GetComponentsInChildren<SkinnedMeshRenderer>()[2].materials[1];
		//Color color = Color.black;

	}
}
