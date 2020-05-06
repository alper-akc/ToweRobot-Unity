using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{

	void Start () 
	{
		
	}
	
	void OnGUI () 
	{
		GUI.Label(new Rect((Screen.width / 2)-50, (Screen.height / 2) - 100, 400, 70), "GAME OVER!", "mainMenuTitle");

		if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height/2, 140, 70), "Restart Level"))
		{
			GameGlobals.startTime();
			GameGlobals.gameOver = false;
			Application.LoadLevel(GameGlobals.getCurrentLevel());
		}


		if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height/2 + 80, 140, 70), "Main Menu"))
		{
			GameGlobals.resetLevel();
			GameGlobals.gameOver = false;
			Application.LoadLevel("StartMenuScene");
		}
	}
}
