using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour {
	
	public float maxHealth = 10;
	private float health;
	public float Health
	{
		get { return health; }
		set 
		{ 
			health = value; 
			if (health > maxHealth)
				health = maxHealth;
			else if (health < 0)
				health = 0;
		}
	}
	public bool Alive
	{
		get { return health > 0;}
	}
	
	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Alive && GetComponent<NetworkView>().isMine)
		{
			Die();
			//Network.DestroyPlayerObjects(networkView.viewID.owner);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (tag == "Player" && coll.gameObject.tag == "Enemy" && GetComponent<NetworkView>().isMine)
		{
			AlterHealth(-1);
			coll.gameObject.GetComponent<EnemyScript>().Explode();
		}
	}
	
	public void Die()
	{
		if (tag == "Player")
		{
			Network.RemoveRPCs(GetComponent<NetworkView>().viewID.owner,1);
			Network.RemoveRPCs(GetComponent<NetworkView>().viewID.owner,2);
			Network.Destroy(gameObject.GetComponent<Player>().PlayerName);
			Network.Destroy(gameObject);
		}	
		else if (tag == "Enemy")
		{
			Network.RemoveRPCs(GetComponent<NetworkView>().viewID);
			Network.Destroy(gameObject);
		}
	}
	
	public void AlterHealth(float delta)
	{
		Health += delta;
		GetComponent<NetworkView>().RPC("HealthAltered",RPCMode.Others, GetComponent<NetworkView>().viewID, Health);
	}
	
	[RPC]
	void HealthAltered(NetworkViewID id, float health)
	{
		GameObject combatant = NetworkView.Find(id).gameObject;
		combatant.GetComponent<CombatScript>().Health = health;
		
	}
}
