using UnityEngine;
using UnityEditor;
using FlatLighting;
using System.Collections;

public class FlatLightingTransparentShaderEditor : FlatLightingShaderEditor {

	private MaterialProperty alpha = null;

	protected override void FindProperties(MaterialProperty[] properties) {
		base.FindProperties(properties);
		alpha = FindProperty("_Alpha", properties);
	}

	protected override void ShaderPropertiesGUI() {
		ShowAlphaProperty();
		base.ShaderPropertiesGUI();
	}

	private void ShowAlphaProperty() {
		using (new UITools.GUIVertical(UITools.VGroupStyle)) {
			base.materialEditor.ShaderProperty(alpha, Labels.Alpha);
		}
	}
}
