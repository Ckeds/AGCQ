using UnityEngine;
using System.Collections;

public class BaseWall : BaseDefense
{
    protected int armor;
    new public void Awake()
    {
        base.Awake();
	}
	
	new public void Update () 
    {
        base.Update();
	}
}
