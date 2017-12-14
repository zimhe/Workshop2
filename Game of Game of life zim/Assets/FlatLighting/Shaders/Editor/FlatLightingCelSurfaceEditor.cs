using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using FlatLighting;

//Note: version 5.3.x of Unity could not manage a ShaderGUI class to be in other than the default namespace, sorry :(
public class FlatLightingCelSurfacerEditor : FlatLightingSurfacerEditor {

	private MaterialProperty celThreshold = null;
	private MaterialProperty celDivisions = null;

	protected override void FindProperties (MaterialProperty[] props) {
		base.FindProperties(props);
		celThreshold = FindProperty("_CelThreshold", props);
		celDivisions = FindProperty("_CelDivisions", props);
	}

	protected override void ShaderPropertiesGUI() {
		ShowCelThresholdProperty();
		base.ShaderPropertiesGUI();
	}

	private void ShowCelThresholdProperty() {
		using (new UITools.GUIVertical(UITools.VGroupStyle)) {
			materialEditor.ShaderProperty(celThreshold, Labels.CelThreshold);
			materialEditor.ShaderProperty(celDivisions, Labels.CelDivisions);
		}
	}
}
