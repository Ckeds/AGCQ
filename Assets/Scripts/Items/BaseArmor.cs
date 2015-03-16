using UnityEngine;
using System.Collections;

public class BaseArmor : BaseItem {

	protected int physDefense;
	protected int acidDefense;
	protected int coldDefense;
	protected int elecDefense;
	protected int fireDefense;

	// Use this for initialization
	new public void Start () 
	{
		type = "armor";
	}
	
	// Update is called once per frame
	public void Update () 
	{
	
	}
}
