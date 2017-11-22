using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh2 : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = CreateTexturedQuad();
	}


    Mesh CreateTri()
    {
        Mesh mesh = new Mesh();

        // 1) create vertex attributes

        Vector3[] positions = new Vector3[3];
        positions[0] = new Vector3(0f, 0f, 0f);
        positions[1] = new Vector3(1f, 0f, 0f);
        positions[2] = new Vector3(0f, 1f, 0f);

        Vector2[] texCoords = new Vector2[3];
        texCoords[0] = new Vector2(0f, 0f);
        texCoords[1] = new Vector2(1f, 0f);
        texCoords[2] = new Vector2(0f, 0.5f);

        // 2) create indices

        int[] tris = new int[] { 0, 1, 2 };

        // 3) assign to mesh

        mesh.vertices = positions;
        mesh.uv = texCoords;
        mesh.SetTriangles(tris, 0);

        return mesh;
    }


    /// <summary>
    /// Creates a colorful unit quadrilateral
    /// </summary>
    /// <returns></returns>
    Mesh CreateQuad()
    {
        Mesh mesh = new Mesh();

        // 1) create vertex attributes

        Vector3[] positions = new Vector3[4];
        positions[0] = new Vector3(0f, 0f, 0f);
        positions[1] = new Vector3(1f, 0f, 0f);
        positions[2] = new Vector3(0f, 1f, 0f);
        positions[3] = new Vector3(1f, 1f, 0f);

        Color[] colors = new Color[4];
        colors[0] = new Color(1f, 0f, 0.5f, 1);    
        colors[1] = new Color(1f, 1f, 0.5f, 1);
        colors[2] = new Color(0f, 1f, 0.5f, 1);
        colors[3] = new Color(0f, 0f, 0.5f, 1);

        // 2) create indices
  
        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        // 3) assign to mesh

        mesh.vertices = positions;
        mesh.colors = colors;
        mesh.SetTriangles(tris, 0);
       
        return mesh;
    }


    /// <summary>
    /// Creates a colorful unit quadrilateral
    /// </summary>
    /// <returns></returns>
    Mesh CreateTexturedQuad()
    {
        Mesh mesh = new Mesh();

        // 1) create vertex attributes

        Vector3[] positions = new Vector3[4];
        positions[0] = new Vector3(0f, 0f, 0f);
        positions[1] = new Vector3(1f, 0f, 0f);
        positions[2] = new Vector3(0f, 1f, 0f);
        positions[3] = new Vector3(1f, 1f, 0f);

        Vector2[] texCoords = new Vector2[4];
        texCoords[0] = new Vector2(0f, 0f);
        texCoords[1] = new Vector2(1f, 0f);
        texCoords[2] = new Vector2(0f, 1f);
        texCoords[2] = new Vector2(1f, 1f);

        // 2) create indices

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        // 3) assign to mesh

        mesh.vertices = positions;
        mesh.uv = texCoords;
        mesh.SetTriangles(tris, 0);

        return mesh;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="facesX"></param>
    /// <param name="facesY"></param>
    /// <returns></returns>
    Mesh CreateGrid(int facesX, int facesY)
    {
        // TODO
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Mesh CreateTetra()
    {
        var positions = new Vector3[]
            {
            new Vector3(0f,0f,0f),
            new Vector3(1f,0f,1f),
            new Vector3(1f,1f,0f),
            new Vector3(0f,1f,1f)
            };

        var colors = new Color[]
        {
            new Color(0f,0f,0.5f),
            new Color(1f,0f,0.5f),
            new Color(0f,1f,0.5f),
            new Color(1f,1f,0.5f)
        };

        var tris = new int[] 
        {
            2, 1, 0,
            2, 0, 3,
            3, 1, 2,
            0, 1, 3
        };

        var mesh = new Mesh();

        mesh.vertices = positions;
        mesh.colors = colors;
        mesh.SetTriangles(tris, 0);

        return mesh;
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
