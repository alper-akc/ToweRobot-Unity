using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour 
{
	public Texture2D backdrop;

	private bool isLoading = false;

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	void OnGUI()
	{
		GUIStyle backgroundStyle = new GUIStyle();

		backgroundStyle.normal.background = backdrop;

		GUI.Label(new Rect((Screen.width - (Screen.height * 2)) * 0.75f, 0, Screen.height * 2f, Screen.height), "", backgroundStyle);

		GUI.Label(new Rect((Screen.width / 2) - 197, 200, 400, 100), "JunkRobot", "mainMenuTitle");

		if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height - 160, 140, 70), "Play"))
		{
			isLoading = true;
			Application.LoadLevel(GameGlobals.getCurrentLevel());
			GameGlobals.startTime();
		}

		bool isWebPlayer = (Application.platform == RuntimePlatform.OSXWebPlayer ||
			Application.platform == RuntimePlatform.WindowsWebPlayer);

		if (!isWebPlayer)
		{
			if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height - 80, 140, 70), "Quit"))
				Application.Quit();
		}

		if (isLoading)
			GUI.Label(new Rect((Screen.width / 2) - 110, (Screen.height / 2) - 60, 400, 70),
			"Loading...", "mainMenuTitle");

	}
}

