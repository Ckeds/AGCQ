using UnityEngine;
using System.Collections;

public class BaseWall : BaseDefense
{
    protected int armor;
    new public void Start()
    {
        base.Start();
	}
	
	new public void Update () 
    {
        base.Update();
	}
}
