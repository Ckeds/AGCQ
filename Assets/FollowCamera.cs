using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	public CharacterMove target;
	Vector3 previousMovement;
	Vector3 currentMovement;
	
	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			currentMovement = target.movement;
			if (currentMovement != previousMovement)
			{
				currentMovement += (previousMovement * 49);
				currentMovement = currentMovement / 50;
			}
			this.transform.position = target.transform.position + currentMovement *30f - transform.forward * 10f;
			//Debug.Log ("OBJECT: " + target.transform.position);
			//Debug.Log ("CAMERA : " + this.transform.position);
			//Debug.Log ("MOVEMENT: " + target.movement *10f);
			previousMovement = currentMovement;
		}
		
	}
}
