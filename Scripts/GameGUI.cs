using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{
    private TowerAI[] towers;
    private EnemyAI[] patrollerDetectors;
	private PatrollerAI[] patrollerAI;

	private float remainingTime;

	//public GUISkin skin;

	private string health, collected;

    public void Start()
    {
        towers = GameObject.Find("Towers").GetComponentsInChildren<TowerAI>();
        patrollerDetectors = GameObject.Find("Patrollers").GetComponentsInChildren<EnemyAI>();
		patrollerAI = GameObject.Find("Patrollers").GetComponentsInChildren<PatrollerAI>();
    }
	
	public void OnGUI () 
    {
        GUI.BeginGroup(new Rect(10, 10, Screen.width - 10, Screen.height - 10));

		GameObject player = GameObject.Find("Junkman").gameObject;
		Junkman junkman = player.GetComponent<Junkman>();

		health = "Health: " + junkman.getHealth();
		collected = "Collected: " + junkman.getNumberOfCollected() + "/" + GameGlobals.totalJunks;
		//points = "Points: " + junkman.getPoints();

		GUI.Box(new Rect(0, 0, 100, 20),health);

		GUI.Box(new Rect(0, 25, 100, 20), collected);

		//GUI.Box(new Rect(0, 50, 100, 20), points);

        foreach (TowerAI com in towers)
        {
			if (com.isPlayerDetected())
			{
				GUI.Box(new Rect(0, 50, 100, 20), "Detected!");
			}
        }

        foreach (EnemyAI pat in patrollerDetectors)
        {
			if (pat.isPlayerDetected())
			{
				GUI.Box(new Rect(0, 50, 100, 20), "Detected!");
			}
        }

		if(junkman.isShieldActive())
			GUI.Box(new Rect(0, 75, 100, 20), "Shield Active!");

		if(junkman.isNitroActive())
			GUI.Box(new Rect(0, 100, 100, 20), "Nitro Active!");

		if (patrollerAI[0].isStunned())
			GUI.Box(new Rect(0, 125, 100, 20), "Enemies Stunned!");

		//GUI.Box(new Rect(Screen.width - 120, 0, 100, 20), "Speed: " + junkman.getSpeed().ToString());

		GUI.Box(new Rect(Screen.width - 120, 0, 100, 20), "Time: " + (GameGlobals.remainingTime));

		GUI.EndGroup();

	}

	void gameOver()
	{
		GameGlobals.stopTime();
		Application.LoadLevel("GameOverScene");
	}

    public void Update()
    {
		remainingTime = GameGlobals.remainingTime;

		if (remainingTime <= 0)
		{
			GameGlobals.gameOver = true;
			Invoke("gameOver", 1);
		}
    }

	//private Vector3[] getTrianglePoints(Vector3 point, float angle, float radius)
	//{
	//    float radAngle = ((90 - (angle/2)) * 2 * Mathf.PI) / 360;

	//    Vector3[] points = new Vector3[2];

	//    points[0] = new Vector3(point.x - (radius * Mathf.Cos(radAngle)), point.y, point.z + (radius * Mathf.Sin(radAngle)));
	//    points[1] = new Vector3(point.x + (radius * Mathf.Cos(radAngle)), point.y, point.z + (radius * Mathf.Sin(radAngle)));

	//    //print("angle: " + radAngle + " cos: " + Mathf.Cos(radAngle) + " sin: " + Mathf.Sin(radAngle));

	//    return points;
	//}

}
