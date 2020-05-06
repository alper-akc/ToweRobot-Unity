using UnityEngine;
using System.Collections;

public class ItemText : MonoBehaviour
{
	public void Update()
	{
		GameObject textObj = GameObject.Find("ShieldText");

		Vector3 camPosition = GameGlobals.getMainCamera().transform.position;

		Vector3 objPosition = this.transform.position;

		float x_dif = objPosition.x - camPosition.x;

		float z_dif = objPosition.z - camPosition.z;

		Vector3 reflection = new Vector3(objPosition.x + x_dif, camPosition.y, objPosition.z + z_dif); 

		this.transform.LookAt(reflection);
	}
}