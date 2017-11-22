using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_2 : MonoBehaviour {

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
        SetupNeighbors3d();


    }
	
	// Update is called once per frame
	void Update () {

        VonNeumannLookup();
        MooreLookup();

    }

    /// <summary>
    /// TESTING VON NEUMANN NEIGHBORS
    /// We can look at the specific voxels above,below,left,right,front,back and color....
    /// We can get all von neumann neighbors and color
    /// </summary>
    /// 
    void VonNeumannLookup()
    {
        //color specific voxel in the grid - [1,1,1]
        GameObject voxel_1 = voxelGrid[1, 1, 1];
        voxel_1.GetComponent<Voxel>().SetState(1);
        voxel_1.GetComponent<Voxel>().VoxelDisplay(1, 0, 0);
        
        //color specific voxel in the grid - [10,10,10]
        GameObject voxel_2 = voxelGrid[10, 10, 10];
        voxel_2.GetComponent<Voxel>().SetState(1);
        voxel_2.GetComponent<Voxel>().VoxelDisplay(1, 0, 0);

        //get neighbor right and color green
        Voxel voxel_1right = voxel_1.GetComponent<Voxel>().getVoxelRight();
        voxel_1right.SetState(1);
        voxel_1right.VoxelDisplay(0, 1, 0);
        
        //get neighbor above and color green
        Voxel voxel_1above = voxel_1.GetComponent<Voxel>().getVoxelAbove();
        voxel_1above.SetState(1);
        voxel_1above.VoxelDisplay(1, 0, 1);

        //get neighbor above and color magenta
        Voxel voxel_2above = voxel_2.GetComponent<Voxel>().getVoxelAbove();
        voxel_2above.SetState(1);
        voxel_2above.VoxelDisplay(1, 0, 1);

        //get all VN neighbors of a cell and color yellow
        //color specific voxel in the grid - [12,12,12]
        GameObject voxel_3 = voxelGrid[12, 12, 12];
        Voxel[] tempVNNeighbors = voxel_3.GetComponent<Voxel>().getNeighbors3dVN();
        foreach (Voxel vox in tempVNNeighbors)
        {
            vox.SetState(1);
            vox.VoxelDisplay(1, 1, 0);
        }
        

    }

    /// <summary>
    /// TESTING MOORES NEIGHBORS
    /// We can look at the specific voxels above,below,left,right,front,back and color....
    /// We can get all von neumann neighbors and color
    /// </summary>
    /// 
    void MooreLookup()
    {
        //get all MO neighbors of a cell and color CYAN
        //color specific voxel in the grid - [14,14,14]
        GameObject voxel_1 = voxelGrid[14, 14, 14];
        Voxel[] tempMONeighbors = voxel_1.GetComponent<Voxel>().getNeighbors3dMO();
        foreach (Voxel vox in tempMONeighbors)
        {
            vox.SetState(1);
            vox.VoxelDisplay(0, 1, 1);
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
    /// SETUP MOORES & VON NEUMANN 3D NEIGHBORS
    /// </summary>
    void SetupNeighbors3d()
    {
        for (int i = 1; i < width-1; i++)
        {
            for (int j = 1; j < length-1; j++)
            {
                for (int k = 1; k < height-1; k++)
                {
                    //the current voxel we are looking at...
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    ////SETUP Von Neumann Neighborhood Cells////
                    Voxel[] tempNeighborsVN = new Voxel[6];

                    //left
                    Voxel VoxelLeft = voxelGrid[i - 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLeft);
                    tempNeighborsVN[0] = VoxelLeft;

                    //right
                    Voxel VoxelRight = voxelGrid[i + 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelRight(VoxelRight);
                    tempNeighborsVN[2] = VoxelRight;

                    //back
                    Voxel VoxelBack = voxelGrid[i, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBack(VoxelBack);
                    tempNeighborsVN[3] = VoxelBack;

                    //front
                    Voxel VoxelFront = voxelGrid[i, j + 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelFront(VoxelFront);
                    tempNeighborsVN[1] = VoxelFront;

                    //below
                    Voxel VoxelBelow = voxelGrid[i, j, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBelow(VoxelBelow);
                    tempNeighborsVN[4] = VoxelBelow;

                    //above
                    Voxel VoxelAbove = voxelGrid[i, j, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(VoxelAbove);
                    tempNeighborsVN[5] = VoxelAbove;

                    //Set the Von Neumann Neighbors [] in this Voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dVN(tempNeighborsVN);

                    ////SETUP Moore's Neighborhood////
                    Voxel[] tempNeighborsMO = new Voxel[26];

                    int tempcount = 0;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            for (int p = -1; p < 2; p++)
                            {
                                if ((i + m >= 0) && (i + m < width) && (j + n >= 0) && (j + n < length) && (k + p >= 0) && (k + p < height))
                                {
                                    GameObject neighborVoxelObj = voxelGrid[i + m, j + n, k + p];
                                    if (neighborVoxelObj != currentVoxelObj)
                                    {
                                        Voxel neighborvoxel = voxelGrid[i + m, j + n, k + p].GetComponent<Voxel>();
                                        tempNeighborsMO[tempcount] = neighborvoxel;
                                        tempcount++;
                                    }
                                }
                            }
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dMO(tempNeighborsMO);
                }
            }
        }
    }

    void updateDensities()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    GameObject currentVoxel = voxelGrid[i, j, k];

                }
            }
        }
    }
}
