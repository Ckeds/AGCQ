using UnityEngine;
using System.Collections;

public class NameMover : MonoBehaviour {
	// Use this for initialization
	GameObject objectToFollow;
	float syncDelay = 0f;
	float syncTime = 0f;
	float lastSyncTime = 0f;
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine) {
			FollowCharacter();
		} 
		else {
			SyncFollow();
		}
	}
	private void FollowCharacter()
	{
		this.transform.position = objectToFollow.transform.position + 
			new Vector3 (0, 0.6f, 0);
	}
	private void SyncFollow()
	{
		syncTime += Time.deltaTime;
		rigidbody2D.position = Vector3.Lerp(syncStartPosition, syncEndPosition , syncTime / syncDelay);
	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{	
		Vector3 networkPosition = Vector3.zero;
		Vector3 networkVelocity = Vector3.zero;

		if (stream.isWriting)
		{
			networkPosition = rigidbody2D.position;
			networkVelocity = rigidbody2D.velocity;
					
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);			
		}
		else
		{		
			stream.Serialize(ref networkPosition);
			stream.Serialize(ref networkVelocity);			
			
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;
			
			syncStartPosition = rigidbody2D.position;
			syncEndPosition = networkPosition + networkVelocity * syncDelay;
		}
	}
	[RPC]
	void CreateName(NetworkViewID id, string name)
	{
		this.GetComponent<TextMesh> ().text = name;
		objectToFollow = NetworkView.Find (id).gameObject;
		if (networkView.isMine) {
			Debug.Log ("Solution is plausible");
			renderer.enabled = false;
		}
	}
}
