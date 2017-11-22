using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	// VARIABLES

	// Texture to be used as start of CA input
	public Texture2D seedImage;
    
	// Number of frames to run which is also the height of the CA
	public int timeEnd = 100;
	int currentFrame = 0;

    //variables for size of the 3d grid
	int width;
	int length;
	int height;

    // Array for storing voxels
    GameObject[,,] voxelGrid;

	// Reference to the voxel we are using
	public GameObject voxelPrefab;

	// Spacing between voxels
	float spacing = 1.0f;

	// Voxel trace line points - variables for drawing lines through the grid
	List<Vector3> linePoints;
	public GameObject tracedLines;
	public Color tracedLinesColorStart = Color.red;
	public Color tracedLinesColorEnd = Color.blue;

    //boolean switches
    //toggles pausing the game
    bool pause = false;


	// FUNCTIONS

	// Use this for initialization
	void Start () {
		// Read the image width and height
		width = seedImage.width;
		length = seedImage.height;
		height = timeEnd;
        // Create a new CA grid
        CreateGrid ();
        //setupNeighbors3d();

    }
	
	// Update is called once per frame
	void Update () {
        // Calculate the CA state, save the new state, display the CA and increment time frame
        if (currentFrame < timeEnd - 1)
        {
            if (pause == false)
            {

            
            // Calculate the future state of the voxels
            CalculateCA();
            // Update the voxels that are printing
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    GameObject currentVoxel = voxelGrid[i, j, 0];
                    currentVoxel.GetComponent<Voxel>().UpdateVoxel();
                }

            }
            // Save the CA state
            SaveCA();

                // Increment the current frame count
            currentFrame++;
            }

            // Display the printed voxels
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 1; k < height; k++)
                    {
                        voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
                    }
                }
            }
        }
        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else 
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());
            }
        }

        //toggle pause with "p" key
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pause == false)
            {
                pause = true;
            }
            else
            {
                pause = false;
            }
        }


    }

	// Create grid function
	void CreateGrid(){
		// Allocate space in memory for the array
		voxelGrid = new GameObject[width, length, height];
		// Populate the array with voxels from a base image
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for (int k = 0; k < height; k++) {
					// Create values for the transform of the new voxel
					Vector3 currentVoxelPos = new Vector3 (i*spacing,k*spacing,j*spacing);
					Quaternion currentVoxelRot = Quaternion.identity;
                    //create the game object of the voxel
					GameObject currentVoxelObj = Instantiate (voxelPrefab, currentVoxelPos, currentVoxelRot);
                    //run the setupVoxel() function inside the 'Voxel' component of the voxelPrefab
                    //this sets up the instance of Voxel class inside the Voxel game object
                    currentVoxelObj.GetComponent<Voxel>().SetupVoxel(i,j,k,1);

                    // Set the state of the voxels
                    if (k == 0) {						
						// Create a new state based on the input image
						int currentVoxelState = (int)seedImage.GetPixel (i, j).grayscale;
                        currentVoxelObj.GetComponent<Voxel> ().SetState (currentVoxelState);
					} else {
                        // Set the state to death
                        currentVoxelObj.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxelObj.GetComponent<Voxel> ().SetState (0);
					}
					// Save the current voxel in the voxelGrid array
					voxelGrid[i,j,k] = currentVoxelObj;
                    // Attach the new voxel to the grid game object
                    currentVoxelObj.transform.parent = gameObject.transform;
				}
			}
		}
	}

	// Calculate CA function
	void CalculateCA(){
		// Go over all the voxels stored in the voxels array
		for (int i = 1; i < width-1; i++) {
			for (int j = 1; j < length-1; j++) {
				GameObject currentVoxelObj = voxelGrid[i,j,0];
				int currentVoxelState = currentVoxelObj.GetComponent<Voxel> ().GetState ();
				int aliveNeighbours = 0;

				// Calculate how many alive neighbours are around the current voxel
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						GameObject currentNeigbour = voxelGrid [i + x, j + y,0];
						int currentNeigbourState = currentNeigbour.GetComponent<Voxel> ().GetState();
						aliveNeighbours += currentNeigbourState;
					}
				}
				aliveNeighbours -= currentVoxelState;
				// Rule Set 1: for voxels that are alive
				if (currentVoxelState == 1) {
					// If there are less than two neighbours I am going to die
					if (aliveNeighbours < 2) {
                        currentVoxelObj.GetComponent<Voxel> ().SetFutureState (0);
					}
					// If there are two or three neighbours alive I am going to stay alive
					if(aliveNeighbours == 2 || aliveNeighbours == 3){
                        currentVoxelObj.GetComponent<Voxel> ().SetFutureState (1);
					}
					// If there are more than three neighbours I am going to die
					if (aliveNeighbours > 3) {
                        currentVoxelObj.GetComponent<Voxel> ().SetFutureState (0);
					}
				}
				// Rule Set 2: for voxels that are death
				if(currentVoxelState == 0){
					// If there are exactly three alive neighbours I will become alive
					if(aliveNeighbours == 3){
                        currentVoxelObj.GetComponent<Voxel> ().SetFutureState (1);
					}
				}
			}
		}
	}

    // Save the CA states
	void SaveCA(){
		for(int i =0; i< width; i++){
			for (int j = 0; j < length; j++) {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];
                savedVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);                
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1) {
                    int currentVoxelAge = currentVoxelObj.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);
                }
			}
		}
	}

    /// <summary>
    /// SETUP AND STORE MOORES AND VON NEUMANN NEIGHBORS
    /// </summary>
    void setupNeighbors3d()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    //setup Von Neumann Neighborhood Cells


                    //setup Moore's Neighborhood
                    GameObject currentVoxelObj = voxelGrid[i, j, k];
                    for (int m = 0; m < height; m++)
                    {
                        for (int n = 0; n < height; n++)
                        {
                            for (int p = 0; p < height; p++)
                            {
                                if ((i + m >= 0) && (i + m < width) && (j + n >= 0) && (j + n < length) && (k + p >= 0) && (k + p < height))
                                {
                                    GameObject neighborVoxelObj = voxelGrid[i + m, j + n, k + p];
                                    if (neighborVoxelObj != currentVoxelObj)
                                    {
                                        //Voxel neighborvoxel = voxelGrid[i + m, j + n, k + p].GetComponent<Voxel>();
                                        currentVoxelObj.GetComponent<Voxel>().neighbors3d_MO.Add(neighborVoxelObj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
