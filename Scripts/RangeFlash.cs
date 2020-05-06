using UnityEngine;
using System.Collections;

public class RangeFlash : MonoBehaviour 
{
	private int sign;

	void Start () 
	{
		sign = -1;
	}
	
	void Update () 
	{
		float alpha = GetComponent<MeshRenderer>().materials[0].color.a + (sign * 0.008f);

		if (alpha <= 0.1f)
			sign = 1;

		if (alpha >= 0.38f)
			sign = -1;

		float r = GetComponentInChildren<MeshRenderer>().materials[0].color.r;
		float g = GetComponentInChildren<MeshRenderer>().materials[0].color.g;
		float b = GetComponentInChildren<MeshRenderer>().materials[0].color.b;

		Color color = new Color(r, g, b, alpha);

		GetComponent<MeshRenderer>().materials[0].color = color;

		//print("color: " + color.ToString() + " alpha: " + alpha + " name: " + GetComponent<MeshRenderer>().materials[0].name);
	}
}
