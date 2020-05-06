using UnityEngine;
using System.Collections;

public class IntruderAlert : MonoBehaviour 
{

	public void playAlertSound()
	{
		this.gameObject.GetComponent<AudioSource>().Play();
	}

	public void stopAlertSound()
	{
		this.gameObject.GetComponent<AudioSource>().Stop();
	}

	void Start () 
	{
	
	}
	

	void Update () 
	{
	
	}
}
