using UnityEngine;
using System.Collections;

public class SpinAnimation : MonoBehaviour {

	void Update () {
		transform.RotateAround(transform.position, transform.up + transform.right, Time.deltaTime * 90f);
	}
}
