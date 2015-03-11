using UnityEngine;
using System.Collections;

public class BaseDefense : WorldObject 
{
    public override void Start()
    {
        isDamageable = true;

    }
    public override void Update()
    {
        base.Update();
    }
    public void Build()
    {

    }
    public void Repair(int repairValue)
    {
        currentHealth += repairValue;
    }
}
