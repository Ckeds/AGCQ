using UnityEngine;
using System.Collections;

public class BaseAccessory : BaseItem {

	protected int physDefense;
	protected int acidDefense;
	protected int coldDefense;
	protected int elecDefense;
	protected int fireDefense;

	// Use this for initialization
	new public void Start () 
	{
		type = "accessory";
	}
	
	// Update is called once per frame
	new public void Update () 
	{
	
	}
}
