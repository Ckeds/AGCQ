using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject nameHolder;
	public GameObject enemyPrefab;
	public float score;
	
	private GameObject[] spawnPoints;
	private float timer = 0;
	private float timerUpdate = 0;
	public float enemyFrequency = 4;
	private bool spawnEnemies = false;
	private int maxEnemies = 300;
	private int spawned;
	public int Swapned
	{
		get{return spawned;}
	}
	public bool SpawnEnemies
	{
		get {return spawnEnemies;}
		set
		{
			networkView.RPC("SpawningEnemies",RPCMode.All, value);
		}
	}
	
	public bool PlayerAlive
	{
		get 
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < players.Length; i++)
			{
				if (players[i].networkView.isMine)
					return true;
			}
			return false;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		timerUpdate += Time.deltaTime;
		if (timer > enemyFrequency)
		{
			timer = 0;
			if (Network.isServer && spawnEnemies && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
			{
				SpawnEnemy();
			}
		}
		if (spawnEnemies && timerUpdate >= 1)
		{
			score = 1/Time.deltaTime;
			score = (int)score;
			timerUpdate = 0;
		}
	}
	
	public void SpawnObject(GameObject prefab, bool networked, Vector3 position, Quaternion rotation, int groupNum)
	{
		if (networked)
		{
			Network.Instantiate( prefab, position, rotation, 0);
		}
		else
		{
			
		}
	}
	
	public void SpawnObject(GameObject prefab, bool networked, int spawnNum, int groupNum)
	{
		SpawnObject(prefab, networked, spawnPoints[spawnNum].transform.position, spawnPoints[spawnNum].transform.rotation, groupNum);
	}
	
	public void SpawnPlayer()
	{
		GameObject player = (GameObject) Network.Instantiate(playerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, 
		                                                     Quaternion.identity, 1);
		GameObject name = (GameObject)Network.Instantiate (nameHolder, player.transform.position + new Vector3(0,0.6f,0),
		                                                   Quaternion.identity, 2);
		Camera.main.GetComponent<FollowCamera> ().target = player.GetComponent<Player>();
		GetComponent<GUIManager> ().MyPlayer = player.GetComponent<Player> ();
		name.networkView.RPC ("CreateName", RPCMode.AllBuffered, player.networkView.viewID,
		                      PlayerPrefs.GetString ("playerName"));
		player.networkView.RPC("SetupPlayer", RPCMode.AllBuffered, player.networkView.viewID);
	} 
	
	public void SpawnEnemy()
	{
		SpawnObject(enemyPrefab, true, Random.Range(0, spawnPoints.Length), 3);
		spawned++;
	}
	
	[RPC]
	public void SpawningEnemies(bool spawn)
	{
		spawnEnemies = spawn;
		if (spawn && !PlayerAlive)
		{
			SpawnPlayer();
			score = 0;
		}
	}
}