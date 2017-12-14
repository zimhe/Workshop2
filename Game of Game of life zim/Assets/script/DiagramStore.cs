using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiagramStore : MonoBehaviour
{


    public Texture2D[] Diagrams = new Texture2D[6];

    public RawImage ShowDiagram;

    public GameObject getvizmode;

    private int diagramIndex = 0;


    // Use this for initialization
    void Start ()
	{

	    ShowDiagram.texture = Diagrams[0];
	}
	
	// Update is called once per frame
	void Update ()
	{
       diagramIndex = getvizmode.GetComponent<Environment>().getVizmode();

	    ShowDiagram.texture = Diagrams[diagramIndex ];

    }

    
}
