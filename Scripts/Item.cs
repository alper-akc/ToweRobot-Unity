using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	public enum ItemType { shield, speedUp, stunner };

	public ItemType type;

	//public GameObject itemObject;

	protected bool collected;

	void Start()
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

				this.GetComponent<AudioSource>().Play();

				Junkman junkman = GameObject.Find("Junkman").GetComponent<Junkman>();

				switch (type)
				{
					case ItemType.shield:
						junkman.activateShield();
						break;

					case ItemType.speedUp:
						junkman.activateNitro();
						break;

					case ItemType.stunner:
						stunEnemies();
						break;
				}

				Invoke("Disappear", 0.2f);
			}
		}
	}

	private void stunEnemies()
	{
		PatrollerAI[] patrollers = GameObject.Find("Patrollers").GetComponentsInChildren<PatrollerAI>();

		foreach (PatrollerAI pat in patrollers)
		{
			pat.Stun();
		}
	}

	void DisappearAnim()
	{
		GameObject itemObj = this.transform.FindChild("Object").gameObject;

		MeshRenderer obj_renderer = itemObj.GetComponent<MeshRenderer>();

		if (obj_renderer != null)
		{

			for (int i = 0; i < obj_renderer.materials.Length; i++)
			{
				float a = obj_renderer.materials[i].color.a - 0.03f;
				float r = obj_renderer.materials[i].color.r;
				float g = obj_renderer.materials[i].color.g;
				float b = obj_renderer.materials[i].color.b;

				Color color = new Color(r, g, b, a);

				obj_renderer.materials[i].color = color;
			}
		}

		MeshRenderer[] child_renderer = itemObj.GetComponentsInChildren<MeshRenderer>();

		for (int i = 0; i < child_renderer.Length; i++)
		{
			for (int j = 0; j < child_renderer[i].materials.Length; j++)
			{
				float a = child_renderer[i].materials[j].color.a - 0.03f;
				float r = child_renderer[i].materials[j].color.r;
				float g = child_renderer[i].materials[j].color.g;
				float b = child_renderer[i].materials[j].color.b;

				Color color = new Color(r, g, b, a);

				child_renderer[i].materials[j].color = color;
			}
		}
	}

	void Disappear()
	{
		this.GetComponentInChildren<Light>().enabled = false;

		Transform halo = this.transform.FindChild("Halo");
		Transform text = this.transform.FindChild("Text");

		if (halo != null && text != null)
		{
			halo.gameObject.active = false;
			text.gameObject.active = false;
		}

		Invoke("Destroy", 5f);
	}

	void Destroy()
	{
		Destroy(gameObject);
	}

	void DisplayText()
	{
		GameObject textObj = this.transform.FindChild("Text").gameObject;

		TextMesh text = textObj.GetComponent<TextMesh>();

		Vector3 currentRot = text.transform.rotation.eulerAngles;

		Vector3 newRot = new Vector3(currentRot.x, GameGlobals.getMainCamera().transform.rotation.y, currentRot.z);

		this.transform.FindChild("Text").gameObject.GetComponent<TextMesh>().transform.rotation = Quaternion.Euler(newRot);
	}

	void Update()
	{
		Turn();

		//DisplayText();

		if (collected)
			DisappearAnim();
	}

	void Turn()
	{
		transform.RotateAround(transform.position, Vector3.up, 60 * Time.deltaTime);
	}

}