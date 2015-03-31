using UnityEngine;
using System.Collections;

public class NameMover : MonoBehaviour {
	// Use this for initialization
	GameObject objectToFollow;
	private Vector3 syncStartPosition;
	private Vector3 syncEndPosition;

	// Update is called once per frame
	void Update () 
	{
		this.transform.position = objectToFollow.transform.position + 
			new Vector3 (0, 0.6f, 0);
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
		objectToFollow.GetComponent<Player> ().PlayerName = this.gameObject;
	}
}
