using UnityEngine;
using System.Collections;

public class BaseDefense : WorldObject 
{
    public override void Awake()
    {
        isDamageable = true;

    }
    public void Update()
    {
        //base.Update();
    }
    public void Build()
    {

    }
    public void Repair(int repairValue)
    {
        currentHealth += repairValue;
    }
}
