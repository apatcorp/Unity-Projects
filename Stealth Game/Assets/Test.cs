using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, (Quaternion.Euler(0f, (Camera.main.fieldOfView) / (2f + Camera.main.nearClipPlane / 2f), 0f) * transform.forward) * 10f);
    }
}
