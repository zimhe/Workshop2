using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using FlatLighting;

//Note: version 5.3.x of Unity could not manage a ShaderGUI class to be in other than the default namespace, sorry :(
public class FlatLightingSurfacerEditor : FlatLightingShaderEditor {

	protected override void ShaderPropertiesGUI() {
		ShowLightingProperties();
		GUILayout.Space (defaultSpace);
		ShowGlobalGradientProperties();
		GUILayout.Space (defaultSpace);
		shouldShowLightSourcesProperties = UITools.GroupHeader(new GUIContent(Labels.LightSourcesHeader), shouldShowLightSourcesProperties);
		if (shouldShowLightSourcesProperties) {
			ShowAmbientLightSettings();
		}
	}
}
