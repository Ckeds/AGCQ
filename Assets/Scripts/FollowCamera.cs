using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	public Player target;
	Vector3 previousMovement;
	Vector3 currentMovement;
	float currentZoom;
	float previousZoom;
	Camera c;
	
	// Use this for initialization
	void Start (){
		c = this.gameObject.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			currentMovement = (target.movement / 2 /* Time.deltaTime*/);
			if (currentMovement != previousMovement)
			{
				currentMovement += (previousMovement * 19);
				currentMovement = currentMovement / 20;
			}
			if (currentMovement.magnitude < 0.0005f)
				currentMovement = new Vector3(0,0,0);
			this.transform.position = target.transform.position + (currentMovement / (10 / c.orthographicSize)) - transform.forward * 5f;
			//Debug.Log ("OBJECT: " + target.transform.position);
			//Debug.Log ("CAMERA : " + this.transform.position);
			//Debug.Log ("MOVEMENT: " + target.movement *10f);
			previousMovement = currentMovement;
			currentZoom = -Input.GetAxis("Mouse ScrollWheel");
			currentZoom += previousZoom;
			if(currentZoom >= .2f)
				currentZoom = .2f;
			c.orthographicSize += currentZoom;
			if(c.orthographicSize >= 6)
				c.orthographicSize = 6;
			if(c.orthographicSize <= 3)
				c.orthographicSize = 3;
			previousZoom = currentZoom / 2;
		}
		
	}
}
