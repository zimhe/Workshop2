using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour {

	public SceneLigtingSetup day;
	public SceneLigtingSetup night;
	public GameObject root;

	void Start() {
		SetLighting(0);
	}

	public void SetLighting(int option) {
		if (option == 0) {
			//day
			night.DisableObjects();
			day.Apply(root);
		} else {
			//night
			night.Apply(root);
			day.DisableObjects();
		} 
	}
}
