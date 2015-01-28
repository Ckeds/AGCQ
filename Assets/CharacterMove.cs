﻿using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
	public Vector3 movement;
    float v = 0;
    float h = 0;
	float previousV = 0;
	float previousH = 0;
    float scale = 0.05f;
    float scaleSprint = 0.075f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        InputMovement();
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
			Debug.Log(h);
		}
        //store Movement
        movement = new Vector3(h, v, 0);

        //following code used to make player character face mouse
        Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);       //Mouse position
        Vector3 objpos = Camera.main.WorldToViewportPoint(transform.position);        //Object position on screen
        Vector2 relobjpos = new Vector2(objpos.x - 0.5f, objpos.y - 0.5f);            //Set coordinates relative to object's center
        Vector2 relmousepos = new Vector2(mouse.x - 0.5f, mouse.y - 0.5f) - relobjpos;//Mouse cursor relative to object's center
        float angle = Vector2.Angle(Vector2.up, relmousepos);                         //Angle calculation

        //if mouse is on the left side of our object
        if (relmousepos.x > 0)
            angle = 360 - angle;

        //apply
        Quaternion rotateValue = Quaternion.AngleAxis(angle, Vector3.forward);
        //Uncomment this line to make the player move based on the mouse cursor
        //movement = rotateValue * movement;
        transform.rotation = rotateValue;
        transform.position += movement;
    }
}
