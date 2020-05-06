using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class GameGlobals : MonoBehaviour 
{
	private const int LEVEL_NUM = 3;

	public static bool gameOver;

	public enum EnemyType { small, medium, big };

	public struct Level
	{
		public string name;
		public float time;

		public Level(string _name, float _time)
		{
			name = _name;
			time = _time;
		}
	};

	public static float remainingTime;

	private static Level[] levels;

	private static int level;

	public static int totalJunks;

	public static float nitroAmount = 400f;
	public static float nitroTime = 10f;

	public static float shieldTime = 10f;

	public static float stunTime = 6f;

	private static Stopwatch levelTimer;

	public static Camera getMainCamera()
	{
		GameObject camobj = GameObject.Find("Main Camera");

		Camera cam = camobj.GetComponent<Camera>();

		return cam;
	}

	void Start () 
	{
		DontDestroyOnLoad(this);

		gameOver = false;

		levelTimer = new Stopwatch();

		levels = new Level[LEVEL_NUM];

		levels[0] = new Level("Level1Scene", 90f);
		levels[1] = new Level("Level2Scene", 90f);
		levels[2] = new Level("Level3Scene", 90f);

		level = 0;
	}

	public static void startTime()
	{
		levelTimer.Start();
	}

	public static void stopTime()
	{
		levelTimer.Stop();
		levelTimer.Reset();
	}

	public static void pauseTime()
	{
		levelTimer.Stop();
	}

	public static void increaseLevel()
	{
		level++;
	}

	public static string getCurrentLevel()
	{
		return levels[level].name;
	}

	public static void resetLevel()
	{
		level = 0;
	}

	public static float getCurrentLevelTime()
	{
		return levels[level].time;
	}

	private void countjunks()
	{
		GameObject junks_obj = GameObject.Find("Junks");

		Transform junks = junks_obj.transform;

		totalJunks = junks.childCount;
	}

	void Update () 
	{
		remainingTime = getCurrentLevelTime() - levelTimer.ElapsedMilliseconds / 1000;
	}


}
