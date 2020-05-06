using UnityEngine;
using System.Collections;

public class lookAtCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 fromCam = (transform.position - Camera.mainCamera.transform.position).normalized;
        //Debug.DrawLine(transform.position, transform.position + fromCam * 10);
        Vector3 myRight = (Camera.mainCamera.transform.TransformDirection(Vector3.right)).normalized;
        //Debug.DrawLine(transform.position, transform.position + myRight * 10);
        Vector3 vecUp = Vector3.Cross(fromCam, myRight).normalized;
        //Debug.DrawLine(transform.position, transform.position + vecUp * 10);

		//Vector3 vecUp = new Vector3(0,1,0);
		//vecUp = Camera.mainCamera.transform.TransformDirection(vecUp);
		transform.LookAt(Camera.main.transform, vecUp);
	}
}
