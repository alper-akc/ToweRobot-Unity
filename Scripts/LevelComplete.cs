using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour 
{

	void Start () 
	{
	
	}

	void OnGUI () 
	{
		GUI.Label(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 100, 400, 70), "LEVEL COMPLETE!", "mainMenuTitle");

		if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height / 2, 140, 70), "Continue"))
		{
			GameGlobals.startTime();
			GameGlobals.gameOver = false;
			Application.LoadLevel(GameGlobals.getCurrentLevel());
		}
	}
}