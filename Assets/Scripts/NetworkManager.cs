﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	//public GameObject map;
	public const int MAX_CLIENTS = 7;
	public const int PORT_NUMBER = 25000;
	public const string GAME_PREFIX = "AGameCalledQuest";
	public const int MAX_NAME_LENGTH = 32;
	public const string DEF_PLAYER_NAME = "Anonymous";
	public const string DEF_GAME_NAME = "";
	public GameObject networkMap;
	
	public enum GameType { Deathmatch };
	GameType gameType = GameType.Deathmatch;
	private string playerName = DEF_PLAYER_NAME;
	public string PlayerName
	{
		get { return playerName; }
		set { playerName = value; }
	}	
	private string gameName = DEF_GAME_NAME;
	public string GameName
	{
		get { return gameName; }
		set { gameName = value; }
	}
	private int maxPlayers = MAX_CLIENTS;
	public int MaxPlayers
	{
		get { return maxPlayers; }
		set { maxPlayers = value; }
	}
	private HostData[] hostList;
	public HostData[] HostList
	{
		get { return hostList; }
	}
	
	SpawnManager spawnManager;
	
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("playerName"))
			playerName = PlayerPrefs.GetString("playerName");
		if (PlayerPrefs.HasKey ("gameName"))
			gameName = PlayerPrefs.GetString("gameName");
		if (PlayerPrefs.HasKey("maxPlayers"))
			maxPlayers = PlayerPrefs.GetInt("maxPlayers");
		//MasterServer.ipAddress = "127.0.0.1";
		
		spawnManager = GetComponent<SpawnManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartServer()
	{
		Network.InitializeServer(maxPlayers, PORT_NUMBER, !Network.HavePublicAddress());
		MasterServer.RegisterHost(GAME_PREFIX + gameType, gameName);
		Network.Instantiate (networkMap, Vector3.zero, Quaternion.identity, 0);
		//g.GetComponent<GUIManager> ().CurrentState = GameObject.Find ("GameManagerGO").GetComponent<GUIManager> ().CurrentState;
		//Destroy(GameObject.Find("GameManagerGO"));
	}
	
	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	public void Disconnect()
	{
		Network.Disconnect();
	}
	
	private void OnServerInitialized()
	{
		Debug.Log("Server Initialized.");
		spawnManager.SpawnPlayer();
	}
	
	private void OnConnectedToServer()
	{
		Debug.Log("Server Joined.");
		spawnManager.SpawnPlayer();
	}
	
	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isServer)
		{
			Debug.Log("Local server connection disconnected");
		}
		else
		{
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
			else
				Debug.Log("Successfully diconnected from the server");
		}
		
		Application.LoadLevel (Application.loadedLevel);
	}
	
	void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("Player " + player + " connected.");
		
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Player " + player + " disconnected.");
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	public void RefreshHostList()
	{
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(GAME_PREFIX + gameType);
	}
	
	private void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}
}
