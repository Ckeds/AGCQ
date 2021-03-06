﻿using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public const int DEF_BUTTON_HEIGHT = 40;

	public int chosenByMouse;
	bool mouseDown;
	
	public enum GUIState {StartScreen, MainMenu, InGame, PauseMenu, CreateGame, 
		JoinGame, EditProfile, Confirmation, DisplayInfo, Loading};

	private Player myPlayer;
	public Player MyPlayer
	{
		get{return myPlayer;}
		set{myPlayer = value;}
	}
	private GUIState currentState = GUIState.Loading;
	public GUIState CurrentState
	{
		get { return currentState; }
		set { currentState = value; }
	}
	private GUIState gotoState;
	private NetworkManager nwm; 
	int selectedHost = 0;
	
	public Rect mainMenuRect;
	public int mainMenuWidth = 320;
	public int mainMenuHeight = 100;
	public int mainButtonHeight = 40;
	
	public Rect createMenuRect;
	public int createMenuWidth = 320;
	public int createMenuHeight = 100;
	public int createButtonHeight = 40;
	
	public Rect joinMenuRect;
	public int joinMenuWidth = 320;
	public int joinMenuHeight = 300;
	public int joinButtonHeight = 40;
	public int joinMenuScrollHeight = 200;
	private Vector2 joinMenuScrollPos = new Vector2();
	
	public Rect pauseMenuRect;
	public int pauseMenuWidth = 320;
	public int pauseMenuHeight = 100;
	public int pauseButtonHeight = 40;
	
	public Rect displayInfoRect;
	public int displayInfoWidth = 320;
	public int displayInfoHeight = 100;
	
	public Rect editProfileRect;
	public int editProfileWidth = 320;
	public int editProfileHeight = 200;
	
	private int leftPad;
	private int topPad;
	private string displayInfo;
	
	public Texture2D noItem;
	public Texture2D noItemHover;
	// Use this for initialization
	void Start () {
		nwm = gameObject.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		//switch to handle input for each GUIState
		switch (currentState)
		{	
		case GUIState.StartScreen:
			if (Input.GetKeyDown(KeyCode.Return))
			{
				currentState = GUIState.MainMenu;
			}
			
			break;
		case GUIState.MainMenu:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentState = GUIState.StartScreen;
			}
			
			break;
		case GUIState.CreateGame:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentState = GUIState.MainMenu;
			}
			
			break;
		case GUIState.JoinGame:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentState = GUIState.MainMenu;
			}
			
			break;
		case GUIState.InGame:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentState = GUIState.PauseMenu;
			}
			
			break;
		case GUIState.PauseMenu:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentState = GUIState.InGame;
			}
			
			break;
		case GUIState.EditProfile:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				nwm.PlayerName = PlayerPrefs.GetString("playerName");
				if (Network.isClient || Network.isServer)
					gotoState = GUIState.PauseMenu;
				else
					gotoState = GUIState.MainMenu;
				currentState = gotoState;
			}
			
			break;
		case GUIState.DisplayInfo:
			if (Input.GetKeyDown(KeyCode.Return))
			{
				currentState = gotoState;
			}
			
			break;
		default:
			
			break;
		}
	}
	
	void OnGUI()
	{
		//Switch for checking state requirements
		switch (currentState)
		{
		default:
			
			break;
		}
		
		//Switch to handle menus
		switch (currentState)
		{
		case GUIState.StartScreen:
			GUILayout.Space(Screen.height / 3);
			GUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width / 3);
			GUILayout.Label("Press Enter to Begin");
			GUILayout.EndHorizontal();
			
			break;
		case GUIState.Loading:
			GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
			GUILayout.Space(3 * Screen.height / 4);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Loading, Please wait...");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label((int)(GameObject.Find("LoadBar/Progress").transform.localScale.x * 100) + "%");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			break;
		case GUIState.MainMenu:
			leftPad = (int) (Screen.width / 2 - mainMenuWidth / 2);
			topPad = (int) (Screen.height / 2 - mainMenuHeight / 2);
			
			mainMenuRect.Set(leftPad, topPad, mainMenuWidth, mainMenuHeight);
			mainMenuRect = GUILayout.Window ((int)GUIState.MainMenu, mainMenuRect, MainMenuWindow, "Main Menu");
			
			break;
		case GUIState.CreateGame:
			leftPad = (int) (Screen.width / 2 - createMenuWidth / 2);
			topPad = (int) (Screen.height / 2 - createMenuHeight / 2);
			
			createMenuRect.Set(leftPad, topPad, createMenuWidth, createMenuHeight);
			createMenuRect = GUILayout.Window ((int)GUIState.CreateGame, createMenuRect, CreateMenuWindow, "Create Game");
			
			break;
		case GUIState.JoinGame:
			leftPad = (int) (Screen.width / 2 - joinMenuWidth / 2);
			topPad = (int) (Screen.height / 2 - joinMenuHeight / 2);
			
			joinMenuRect.Set(leftPad, topPad, joinMenuWidth, joinMenuHeight);
			joinMenuRect = GUILayout.Window ((int)GUIState.JoinGame, joinMenuRect, JoinMenuWindow, "Join Game");
			
			break;
		case GUIState.InGame:

			SpawnManager spawner = GetComponent<SpawnManager>();
			//GUILayout.Label("Framerate: " + GetComponent<SpawnManager>().score);
			//GUILayout.Label("NumEnemies: " + GetComponent<SpawnManager>().Swapned);
			
			if (!spawner.SpawnEnemies && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
			{
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				GUILayout.Space(10);
				GUILayout.TextField("Press Enter To Begin");
				GUILayout.EndHorizontal();
				if (Input.GetKeyDown(KeyCode.Return))
				{
					spawner.SpawnEnemies = true;
				}
			}
			
			break;
		case GUIState.PauseMenu:
			leftPad = (int) (Screen.width / 2 - pauseMenuWidth / 2);
			topPad = (int) (Screen.height / 2 - pauseMenuHeight / 2);
			Event e = Event.current;
			//Debug.Log("Current detected event: " + e);
			if(mouseDown)
			{
				GUI.DrawTexture(new Rect(e.mousePosition.x -23, e.mousePosition.y -23,46,46),
				                myPlayer.items[chosenByMouse].GetComponent<BaseItem>().getThatSprite());
			}
			for (int i = 0; i < myPlayer.items.Length; i++)
			{
				float y = 10 + ((i/10) * 85);
				float x = 10 + ((i%10) * 85);
				Rect position = new Rect(x, y, 76, 76);
				if(myPlayer.items[i] != null && myPlayer.items[i].GetComponent<BaseItem>())
				{
					if(position.Contains(e.mousePosition))
					{
						GUI.DrawTexture(position, myPlayer.items[i].GetComponent<BaseItem>().guiTexHover);
						if(e.button ==  0 && e.isMouse && 
						   e.type == EventType.MouseDown && myPlayer.items[i] != null)
						{
							//Debug.Log("I'm Clicking it!");
							mouseDown = true;
							chosenByMouse = i;
						}
					}
					else
						GUI.DrawTexture(position, myPlayer.items[i].GetComponent<BaseItem>().guiTex);
				}
				else
				{
					if(position.Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(position, noItemHover);
					}
					else
						GUI.DrawTexture(position, noItem);
				}
				if(e.button ==  0 && e.isMouse && e.type == EventType.MouseUp 
				   && i != chosenByMouse && mouseDown && position.Contains(e.mousePosition))
				{
					//Debug.Log(myPlayer.items[i]);
					GameObject tempObject = myPlayer.items[i];
					Debug.Log (tempObject);
					myPlayer.items[i] = myPlayer.items[chosenByMouse];
					myPlayer.items[chosenByMouse] = tempObject;
					mouseDown = false;
					chosenByMouse = -1;
				}
			}
			if(e.button ==  0 && e.isMouse && e.type == EventType.MouseUp)
			{
				Debug.Log("No longer Clicking");
				mouseDown = false;
				chosenByMouse = -1;
			}
			pauseMenuRect.Set(leftPad, topPad, pauseMenuWidth, pauseMenuHeight);
			pauseMenuRect = GUILayout.Window ((int)GUIState.PauseMenu, pauseMenuRect, PauseMenuWindow, "Game Menu");
			
			break;
		case GUIState.DisplayInfo:
			leftPad = (int) (Screen.width / 2 - displayInfoWidth / 2);
			topPad = (int) (Screen.height / 2 - displayInfoHeight / 2);
			
			displayInfoRect.Set(leftPad, topPad, displayInfoWidth, displayInfoHeight);
			displayInfoRect = GUILayout.Window ((int)GUIState.DisplayInfo, displayInfoRect, DisplayInfoWindow, "Notification");
			
			break;
		case GUIState.EditProfile:
			leftPad = (int) (Screen.width / 2 - editProfileWidth / 2);
			topPad = (int) (Screen.height / 2 - editProfileHeight / 2);
			
			editProfileRect.Set(leftPad, topPad, editProfileWidth, editProfileHeight);
			editProfileRect = GUILayout.Window ((int)GUIState.EditProfile, editProfileRect, EditProfileWindow, "Edit Profile");
			
			break;
		default:
			
			break;
		}
	}
	
	void MainMenuWindow(int windowID)
	{
		GUILayout.Label("Profile Name: " + nwm.PlayerName);
		GUILayout.Space(10);
		
		if (GUILayout.Button("Edit Profile", GUILayout.Height(mainButtonHeight)))
		{
			currentState = GUIState.EditProfile;
		}
		
		if (GUILayout.Button("Create Game", GUILayout.Height(mainButtonHeight)))
		{
			currentState = GUIState.CreateGame;
		}
		
		if (GUILayout.Button("Join Game", GUILayout.Height(mainButtonHeight)))
		{
			currentState = GUIState.JoinGame;
			nwm.RefreshHostList();
		}
	}
	
	void CreateMenuWindow(int windowID)
	{
		GUILayout.Label("Room name: ");
		nwm.GameName = GUILayout.TextField(nwm.GameName, NetworkManager.MAX_NAME_LENGTH);
		GUILayout.Label("Max players: " + (nwm.MaxPlayers + 1));
		nwm.MaxPlayers = Mathf.RoundToInt(GUILayout.HorizontalSlider(nwm.MaxPlayers, 1, NetworkManager.MAX_CLIENTS));
		if (GUILayout.Button("Create Game", GUILayout.Height(createButtonHeight)))
		{
			if (nwm.GameName != "")
			{
				PlayerPrefs.SetString("gameName", nwm.GameName);
				PlayerPrefs.SetInt("maxPlayers", nwm.MaxPlayers);
				nwm.StartServer();
			}
			else
			{
				displayInfo = "The room name cannot be left blank!";
				gotoState = GUIState.CreateGame;
				currentState = GUIState.DisplayInfo;
			}
		}
		if (GUILayout.Button("Main Menu", GUILayout.Height(createButtonHeight)))
		{
			currentState = GUIState.MainMenu;
		}
	}
	
	void JoinMenuWindow(int windowID)
	{
		HostData[] hostList = nwm.HostList;
		string[] displayList;
		
		if (GUILayout.Button("Refresh Hosts", GUILayout.Height(joinButtonHeight)))
		{
			nwm.RefreshHostList();
		}
		
		joinMenuScrollPos = GUILayout.BeginScrollView(joinMenuScrollPos, false, true, GUILayout.Height(joinMenuScrollHeight));
		
		if (hostList != null)
		{
			displayList = new string[hostList.Length];
			
			for (int i = 0; i < hostList.Length; i++)
			{
				displayList[i] = hostList[i].gameName + " (" + hostList[i].connectedPlayers + "/" + hostList[i].playerLimit + ")";
			}
			
			if (selectedHost >= displayList.Length)
				selectedHost = 0;
			
			selectedHost = GUILayout.SelectionGrid(selectedHost, displayList, 1);	
		}
		
		GUILayout.EndScrollView();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Join Game", GUILayout.Height(joinButtonHeight)))
		{
			if (hostList != null && hostList.Length >= 1)
			{
				nwm.JoinServer(hostList[selectedHost]);
			}
		}
		
		if (GUILayout.Button("Main Menu", GUILayout.Height(joinButtonHeight)))
		{
			currentState = GUIState.MainMenu;
		}
		GUILayout.EndHorizontal();
	}
	
	void PauseMenuWindow(int windowID)
	{
		if (GUILayout.Button("Resume Game", GUILayout.Height(pauseButtonHeight)))
		{
			currentState = GUIState.InGame;
			Debug.Log (myPlayer.items[0]);
		}
		
		if (GUILayout.Button("Edit Profile", GUILayout.Height(mainButtonHeight)))
		{
			currentState = GUIState.EditProfile;
		}
		
		if (GUILayout.Button("Disconnect", GUILayout.Height(pauseButtonHeight)))
		{
			currentState = GUIState.MainMenu;
			nwm.Disconnect();
		}
	}
	
	void EditProfileWindow(int windowID)
	{		
		GUILayout.Label("Name: ");
		nwm.PlayerName = GUILayout.TextField(nwm.PlayerName, NetworkManager.MAX_NAME_LENGTH);
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save Profile", GUILayout.Height(DEF_BUTTON_HEIGHT)))
		{
			if (nwm.PlayerName != "")
			{
				PlayerPrefs.SetString("playerName", nwm.PlayerName);
				PlayerPrefs.Save();
				
				displayInfo = "Profile Saved.";
				if (Network.isClient || Network.isServer)
					gotoState = GUIState.PauseMenu;
				else
					gotoState = GUIState.MainMenu;
				
				currentState = GUIState.DisplayInfo;
			}
			else
			{
				displayInfo = "Your name must not be left blank!";
				gotoState = GUIState.EditProfile;
				currentState = GUIState.DisplayInfo;
			}
		}
		
		if (GUILayout.Button("Cancel", GUILayout.Height(joinButtonHeight)))
		{
			nwm.PlayerName = PlayerPrefs.GetString("playerName");
			if (Network.isClient || Network.isServer)
				gotoState = GUIState.PauseMenu;
			else
				gotoState = GUIState.MainMenu;
			currentState = gotoState;
		}
		GUILayout.EndHorizontal();
	}
	
	void DisplayInfoWindow(int windowID)
	{
		GUILayout.Label(displayInfo);
		
		if (GUILayout.Button("Continue", GUILayout.Height(DEF_BUTTON_HEIGHT)))
		{
			currentState = gotoState;
		}
	}
	
	private void OnServerInitialized()
	{
		currentState = GUIState.InGame;
		//Screen.lockCursor = true;
	}
	
	private void OnConnectedToServer()
	{
		currentState = GUIState.InGame;
		//Screen.lockCursor = true;
	}
	
	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (currentState != GUIState.MainMenu)
		{
			displayInfo = "Connection to Server Lost.";
			gotoState = GUIState.MainMenu;
			currentState = GUIState.DisplayInfo;
		}			
	}
}