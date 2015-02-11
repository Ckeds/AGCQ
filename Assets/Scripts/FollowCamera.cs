using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	public Player target;
	Vector3 previousMovement;
	Vector3 currentMovement;
	
	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			currentMovement = (target.movement * Time.deltaTime);
			if (currentMovement != previousMovement)
			{
				currentMovement += (previousMovement * 19);
				currentMovement = currentMovement / 20;
			}
			if (currentMovement.magnitude < 0.0005f)
				currentMovement = new Vector3(0,0,0);
			this.transform.position = target.transform.position + currentMovement *30f - transform.forward * 5f;
			//Debug.Log ("OBJECT: " + target.transform.position);
			//Debug.Log ("CAMERA : " + this.transform.position);
			//Debug.Log ("MOVEMENT: " + target.movement *10f);
			previousMovement = currentMovement;
		}
		
	}
}
