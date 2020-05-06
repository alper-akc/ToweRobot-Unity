using UnityEngine;
using System.Collections;

public class TowerRangeCircle : MonoBehaviour 
{

	private void turnRangeCircle()
	{
		//transform.Find("RangeCircle").RotateAround(transform.Find("RangeCircle").position, Vector3.up, 60 * Time.deltaTime);
		Quaternion q = Quaternion.identity;
		q *= Quaternion.Euler(0, 60 * Time.time, 0);
		q *= Quaternion.Euler(90, 0, 0);
		transform.rotation = q;
	}

	void Start ()
	{
		
	}
	
	void Update () 
	{
		turnRangeCircle();
	}
}
