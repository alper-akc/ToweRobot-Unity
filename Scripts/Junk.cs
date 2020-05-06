using UnityEngine;
using System.Collections;

public class Junk : MonoBehaviour 
{
	private int value = 10;

	private bool collected;

	public int getValue()
	{
		return value;
	}

	void Start () 
	{
		collected = false;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (!collected)
			{
				collected = true;

				this.gameObject.GetComponentInChildren<ParticleEmitter>().emit = false;

				this.GetComponent<AudioSource>().Play();

				Junkman junkman = GameObject.Find("Junkman").GetComponent<Junkman>();

				junkman.collectJunk();
				//junkman.updatePoints(value);

				Invoke("Disappear", 0.5f);
			}
		}
	}

	void DisappearAnim()
	{
		float a = GetComponentInChildren<MeshRenderer>().materials[0].color.a - 0.03f;
		float r = GetComponentInChildren<MeshRenderer>().materials[0].color.r;
		float g = GetComponentInChildren<MeshRenderer>().materials[0].color.g;
		float b = GetComponentInChildren<MeshRenderer>().materials[0].color.b;

		Color color = new Color(r, g, b, a);

		GetComponentInChildren<MeshRenderer>().materials[0].color = color;
	}

	void Disappear()
	{
		this.GetComponentInChildren<Light>().enabled = false;
		Transform tmp = this.transform.FindChild("JunkHalo");

		if (tmp != null)
		{
			tmp.gameObject.active = false;
		}

		Invoke("Destroy", 5f);
	}

	void Destroy()
	{
		Destroy(gameObject);
	}
	
	void Update () 
	{
		Turn();

		if(collected)
			DisappearAnim();
	}

	void Turn()
	{
		transform.RotateAround(transform.position,Vector3.up, 60 * Time.deltaTime);
	}
}
