using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */ 

public class CreateMesh : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        var filter = GetComponent<MeshFilter>();
        filter.mesh = CreateColoredQuad();
        //filter.mesh = CreateTexturedQuad();
	}


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Mesh CreateColoredQuad()
    {
        var positions = new Vector3[4]
        {
            new Vector3(-0.5f, -0.5f, 0.0f),
            new Vector3(0.5f, -0.5f, 0.0f),
            new Vector3(-0.5f, 0.5f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f)
        };

        var colors = new Color[4]
        {
            new Color(0.0f,0.0f,0.5f),
            new Color(1.0f,0.0f,0.5f),
            new Color(0.0f,1.0f,0.5f),
            new Color(1.0f,1.0f,0.5f)
        };

        var indices = new int[] { 0, 1, 2, 2, 1, 3};

        var mesh = new Mesh();
        mesh.vertices = positions;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        return mesh;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Mesh CreateTexturedQuad()
    {
        var positions = new Vector3[4]
        {
            new Vector3(-0.5f, -0.5f, 0.0f),
            new Vector3(0.5f, -0.5f, 0.0f),
            new Vector3(-0.5f, 0.5f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f)
        };

        var texCoords = new Vector2[4]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f)
        };

        var indices = new int[] { 0, 1, 2, 2, 1, 3 };

        var mesh = new Mesh();
        mesh.vertices = positions;
        mesh.uv = texCoords;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        return mesh;
    }
}
