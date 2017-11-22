using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDisplay : MonoBehaviour {

    Vector3 centerOfObject;

	// Use this for initialization
	void Start () {
        centerOfObject = new Vector3(0, 0, 0);
        Transform[] childrenTransforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childrenTransforms)
        {
            centerOfObject += childTransform.position;
        }
        centerOfObject = centerOfObject / childrenTransforms.Length;
    }
	
	// Update is called once per frame
	void Update () {        
        transform.RotateAround(centerOfObject, Vector3.up, 20 * Time.deltaTime);
    }
}
