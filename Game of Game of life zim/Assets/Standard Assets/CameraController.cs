using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */ 

public class CameraController : MonoBehaviour
{
    public float OrbitSensitivity = 5.0f;
    public float OrbitStiffness = 10.0f;

    public float ZoomSensitivity = 1.0f;
    public float ZoomStiffness = 5.0f;
    
    private Transform _pivot;
    private Vector3 _rotation;
    private float _distance = 50.0f;


	// Use this for initialization
	void Start ()
    {
        _pivot = transform.parent;
        _distance = -transform.localPosition.z;
        _rotation = _pivot.rotation.eulerAngles;
	}


    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {
            if (Input.GetMouseButton(0))
            {
                _rotation.x -= Input.GetAxis("Mouse Y") * OrbitSensitivity;
                _rotation.y += Input.GetAxis("Mouse X") * OrbitSensitivity;
                _rotation.x = Mathf.Clamp(_rotation.x, -90.0f, 90.0f);
            }

            _distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity * _distance;
            _distance = Mathf.Clamp(_distance, 1.0f, 100.0f);
        
        var q = Quaternion.Euler(_rotation.x, _rotation.y, 0.0f);
        _pivot.rotation = Quaternion.Lerp(_pivot.rotation, q, Time.deltaTime * OrbitStiffness);

        var z = Mathf.Lerp(transform.localPosition.z, -_distance, Time.deltaTime * ZoomStiffness);
        transform.localPosition = new Vector3(0.0f, 0.0f, z);
    }
}
