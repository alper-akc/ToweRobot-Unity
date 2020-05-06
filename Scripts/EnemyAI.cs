using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
	public GameGlobals.EnemyType type;

    protected bool playerDetected;

	private float rateOfFire = 1;

	private int damage;

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
				damage = 10;
				break;

			case GameGlobals.EnemyType.medium:
				damage = 8;
				break;

			case GameGlobals.EnemyType.small:
				damage = 6;
				break;
		}

		GameObject.Find("Main Camera").transform.FindChild("DamageEffect").gameObject.active = false;
    }

	public void fire()
	{
		GameObject player = GameObject.Find("Junkman");
		Junkman jm = player.GetComponent<Junkman>();
		GameObject det_obj = player.transform.Find("DetonatorFire").gameObject;
		Detonator det = det_obj.GetComponent<Detonator>();

		//GameObject go = GameObject.Find("DetonatorFire");
		//Detonator det = go.GetComponent<Detonator>();

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

	public void applyDamage()
	{
		GameObject player = GameObject.Find("Junkman");
		Junkman jm = player.GetComponent<Junkman>();

		if(!jm.isShieldActive())
			jm.updateHealth(-damage);
	}		

    public void OnTriggerEnter(Collider collider)
    {
		PatrollerAI patroller = this.transform.parent.gameObject.GetComponent<PatrollerAI>();

		if (collider.CompareTag("Player"))
		{
			playerDetected = true;

			if (!patroller.isStunned())
			{
				this.transform.parent.Find("PatrollerSound").gameObject.SendMessage("playAlertSound");
				this.SendMessageUpwards("changeDetected", true);
				InvokeRepeating("fire", 0.5f, rateOfFire);
			}
		}
	}

	public void OnTriggerStay(Collider collider)
	{
		if (collider.CompareTag("Player"))
			playerDetected = true;
	}

    public void OnTriggerExit(Collider collider)
    {
		PatrollerAI patroller = this.transform.parent.gameObject.GetComponent<PatrollerAI>();

		if (collider.CompareTag("Player"))
		{
			playerDetected = false;

			if (!patroller.isStunned())
			{
				this.transform.parent.Find("PatrollerSound").gameObject.SendMessage("stopAlertSound");
				this.SendMessageUpwards("changeDetected", false);
				CancelInvoke("fire");
			}
		}
    }

}
