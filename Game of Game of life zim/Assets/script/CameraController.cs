using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


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
    public Transform Target;
    private Vector3  StartView;
    private bool viewRestore = false;
    private bool Orbiting = false;
    
    


    // Use this for initialization
    void Start ()
    {
        StartView = transform.parent.rotation.eulerAngles;
        _pivot = transform.parent;
        _pivot .position = Target.transform.position;


        _rotation = _pivot.rotation.eulerAngles;
        //RestoreView();
	    transform.localPosition = new Vector3(0.0f, 0.0f, -120f);

        _distance = -transform.localPosition.z;
	    
        
    }
  
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        Quaternion currentView = new Quaternion();

        if (Input.GetMouseButtonDown(1) )
        {

            viewRestore = false;
            Orbiting = false;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            viewRestore = false;
        }
        if (viewRestore == true)
        {
            
            var r = Quaternion.Euler(StartView.x, StartView.y, 0.0f);
            _pivot.rotation = Quaternion.Lerp(_pivot .rotation , r, 0.2f);
            var s = Mathf.Lerp(transform.localPosition.z, -120f, 0.1f);
            transform.localPosition = new Vector3(0.0f, 0.0f, s);
            _rotation = _pivot.rotation.eulerAngles;
            _distance = -transform.localPosition.z;
            
        }
        else
        {
            _distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity * _distance;
            _distance = Mathf.Clamp(_distance, 60f, 180f);

            var z = Mathf.Lerp(transform.localPosition.z, -_distance, Time.deltaTime * ZoomStiffness);
            transform.localPosition = new Vector3(0.0f, 0.0f, z);

        }
        if (Orbiting == true)
        {
            _rotation = _pivot.rotation.eulerAngles;
            float OrbitingSpeed = 1f;
            _rotation.y += OrbitingSpeed ;
            var t = Quaternion.Euler(_rotation.x, _rotation.y, 0.0f);
            _pivot.rotation =Quaternion.Lerp(_pivot .rotation  , t, 1f);


        }

        if (Input.GetMouseButton(1))
        {
           
            _rotation.x -= Input.GetAxis("Mouse Y") * OrbitSensitivity;
            _rotation.y += Input.GetAxis("Mouse X") * OrbitSensitivity;
            _rotation.x = Mathf.Clamp(_rotation.x, -5.0f, 75.0f);

            var q = Quaternion.Euler(_rotation.x, _rotation.y, 0.0f);
            _pivot.rotation = Quaternion.Lerp( _pivot .rotation , q, Time.deltaTime * OrbitStiffness);
            
        }
    }

    public void RestoreView()
    {
        viewRestore = true;
        Orbiting = false;
    }
    public void OrbitingView()
    {
        if (Orbiting == false)
        {
            Orbiting = true;
        }
        else
        {
            Orbiting = false;
        }
       
        viewRestore = false ;
    }
}
