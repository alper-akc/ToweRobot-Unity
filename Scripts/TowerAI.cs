using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour 
{
	public GameGlobals.EnemyType type;

	private bool playerDetected;

	private int damage;

	public float rateOfFire = 1.5f;

	public bool isPlayerDetected()
	{
		return playerDetected;
	}

	public void Start()
	{
		playerDetected = false;

		switch (type)
		{
			case GameGlobals.EnemyType.big:
				damage = 20;
				break;

			case GameGlobals.EnemyType.medium:
				damage = 16;
				break;

			case GameGlobals.EnemyType.small:
				damage = 12;
				break;
		}

		GameObject.Find("Main Camera").transform.FindChild("DamageEffect").gameObject.active = false;
	}

	public void applyDamage()
	{
		GameObject player = GameObject.Find("Junkman");
		Junkman jm = player.GetComponent<Junkman>();

		if(!jm.isShieldActive())
			jm.updateHealth(-damage);
	}		


	private void fire()
	{
		GameObject player = GameObject.Find("Junkman");
		Junkman jm = player.GetComponent<Junkman>();
		GameObject det_obj = player.transform.Find("DetonatorFire").gameObject;
		Detonator det = det_obj.GetComponent<Detonator>();

		det.Explode();

		det_obj.GetComponent<AudioSource>().Play();

		applyDamage();
		
		if(!jm.isShieldActive())
			damageEffectOn();
	}

	private void damageEffectOn()
	{
		GameObject.Find("Main Camera").transform.FindChild("DamageEffect").gameObject.active = true;
		Invoke("damageEffectOff", 0.2f);
	}

	private void damageEffectOff()
	{
		GameObject.Find("Main Camera").transform.FindChild("DamageEffect").gameObject.active = false;
	}

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			playerDetected = true;

			this.transform.parent.Find("SirenSound").gameObject.SendMessage("playAlertSound");

			InvokeRepeating("fire", 0.5f, rateOfFire);
		}
	}

	//public void OnTriggerStay(Collider collider)
	//{
	//    if (playerDetected && collider.CompareTag("Player"))
	//    {
	//        //collider.gameObject.rigidbody.WakeUp();
	//        //print("detected: " + playerDetected + " fires: " + fireCount + " in trigger");
	//        //elapsedTime += Time.deltaTime;
	//        //if(elapsedTime > 3)
	//        //{
	//        //InvokeRepeating("fire", 3,10);
	//        //wait();
	//        //print("detected: " + playerDetected + " fires: " + fireCount + " in trigger");
	//    }
	//}

	public void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			//collider.gameObject.rigidbody.WakeUp();
			playerDetected = false;
			transform.parent.Find("SirenSound").gameObject.SendMessage("stopAlertSound");
			CancelInvoke("fire");
			//fireCount = 0;
			//print("exited trigger" + " detected: " + playerDetected);
		}
	}
	
}
