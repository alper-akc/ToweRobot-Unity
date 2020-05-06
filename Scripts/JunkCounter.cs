using UnityEngine;
using System.Collections;

public class JunkCounter : MonoBehaviour 
{

	void Start () 
	{
		countjunks();
	}

	private void countjunks()
	{
		Transform junks = this.transform;

		GameGlobals.totalJunks = junks.childCount;
	}

	void Update () 
	{
	
	}
}
