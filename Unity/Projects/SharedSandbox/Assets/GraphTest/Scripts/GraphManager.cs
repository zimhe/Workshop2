using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpatialSlur.SlurCore;
using SpatialSlur.SlurMesh;

/*
 * Notes
 */ 

/// <summary>
/// 
/// </summary>
public class GraphManager : MonoBehaviour
{
    private HeGraph3d _graph;

    private Mesh _mesh;
    private Vector3[] _positions;
    private Vector3[] _normals;

    private Shader _shader;


    // Use this for initialization
    void Start ()
    {
        _mesh = GetComponent<MeshFilter>().sharedMesh;
        _mesh.MarkDynamic();

        _shader = GetComponent<MeshRenderer>().material.shader;
	}
	

	// Update is called once per frame
	void Update ()
    {
		
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void LoadGraph(string path)
    {
        _graph = HeGraph3d.Factory.CreateFromJson(path);
    }


    /// <summary>
    /// 
    /// </summary>
    private void InitBuffers()
    {

    }
}
