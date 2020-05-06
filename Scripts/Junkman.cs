using UnityEngine;
using System.Collections;

public class Junkman : MonoBehaviour 
{
	private int health;

	private int numCollected;

	//private int points;

	private bool shieldActive;
	private bool nitroActive;

	public float getSpeed()
	{
		return this.gameObject.GetComponent<Vehicle>().getSpeed();
	}

	public bool isShieldActive()
	{
		return shieldActive;
	}

	public bool isNitroActive()
	{
		return nitroActive;
	}

	private void enableObject(string objectName, bool enabled)
	{
		this.transform.FindChild(objectName).gameObject.active = enabled;
	}

	// "Speed Up" methods

	public void activateNitro()
	{
		this.gameObject.GetComponent<Vehicle>().maxSpeed += 35f;
		enableObject("Nitro",true);
		this.transform.FindChild("Nitro").GetComponent<ParticleEmitter>().emit = true;
		changeColor();
		nitroActive = true;

		Invoke("deactivateNitro", GameGlobals.nitroTime);
	}

	private void deactivateNitro()
	{
		this.transform.FindChild("Nitro").GetComponent<ParticleEmitter>().emit = false;
		resetColor();

		this.gameObject.GetComponent<Vehicle>().maxSpeed -= 35f;

		nitroActive = false;
	}

	private void changeColor()
	{
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		for (int i = 0; i < renderer.materials.Length; i++)
		{
			renderer.materials[i].color = Color.yellow;
		}
	}

	private void resetColor()
	{
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		for (int i = 0; i < renderer.materials.Length; i++)
		{
			renderer.materials[i].color = Color.white;
		}
	}

	// "Shield" methods

	public void activateShield()
	{
		enableObject("HollowSphere",true);

		shieldActive = true;

		Invoke("deactivateShield", GameGlobals.nitroTime);
	}

	public void deactivateShield()
	{
		shieldActive = false;

		enableObject("HollowSphere",false);
	}

	// Gameplay methods

	void gameOver()
	{
		GameGlobals.stopTime();
		Application.LoadLevel("GameOverScene");
	}

	void levelComplete()
	{
		GameGlobals.increaseLevel();
		GameGlobals.stopTime();
		Application.LoadLevel("LevelCompleteScene");

	}

	public int getHealth()
	{
		return health;
	}

	public int getNumberOfCollected()
	{
		return numCollected;
	}

	//public int getPoints()
	//{
	//    return points;
	//}

	public void setHealth(int _health)
	{
		health = _health;
	}

	//public void updatePoints(int amount)
	//{
	//    points += amount;
	//}

	public void updateHealth(int amount)
	{
		health += amount;
	}

	public void collectJunk()
	{
		numCollected++;
	}

	// Unity overriden methods

	void Start()
	{
		health = 100;
		numCollected = 0;
		//points = 0;

		enableObject("Nitro",false);
		enableObject("HollowSphere",false);

		shieldActive = false;
	}

	void Update () 
	{
		if (numCollected == GameGlobals.totalJunks)
		{
			GameGlobals.gameOver = true;

			Invoke("levelComplete", 1);
		}

		if (health <= 0)
		{
			this.transform.FindChild("DetonatorExplosion").GetComponent<Detonator>().Explode();
			GameGlobals.gameOver = true;
			GameGlobals.stopTime();
			Invoke("gameOver", 1);
			return;
		}
	}
}
