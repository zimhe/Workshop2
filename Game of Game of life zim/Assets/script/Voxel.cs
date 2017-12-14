using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
    //state
	private int state = 0;
    //next state
    private int futureState = 0;
    //age
    private int age = 0;
    //density3dMO
    private int density3dMO = 0;
    //density3dVN
    private int density3dVN = 0;
    //material property block for setting material properties with renderer
    private MaterialPropertyBlock props;
    //the mesh renderer
    public new MeshRenderer renderer;
    //var stores my 3d address
	public Vector3 address;

    //The Mesh Filter takes a mesh from your assets and passes it to the Mesh Renderer for rendering on the screen
    //One Voxel can contain different meshes which are the representation of different types of voxels
    public MeshFilter type0mesh, type1Mesh, type2Mesh, type3Mesh;

    //variable to store a type for this voxel
	int type;

    //von neumann neighbors
    private Voxel[] neighbors3dVN = new Voxel[6];

    //moore's neighbors
    private Voxel[] neighbors3dMO = new Voxel[26];

    private Voxel voxelAbove;
    private Voxel voxelBelow;
    private Voxel voxelRight;
    private Voxel voxelLeft;
    private Voxel voxelFront;
    private Voxel voxelBack;

    // FUNCTIONS

    public void SetupVoxel(int i, int j, int k, int _type)
    {
        //set reference to time end 
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
        //initially set to false
        renderer.enabled = false;
        //set my address as a vector
		address = new Vector3 (i,j,k);

        //gets the type of this voxel and sets the mesh filter by type - allows us to preload
        //different meshes and render a different mesh for different voxels based on the type
		type = _type;
		switch (type) {
		case 1:
			MeshFilter setMesh = gameObject.GetComponent<MeshFilter> ();
			setMesh = type1Mesh;
			break;
		case 2:
			MeshFilter setMesh2 = gameObject.GetComponent<MeshFilter> ();
			setMesh2 = type2Mesh;
			break;
		case 3:
			MeshFilter setMesh3 = gameObject.GetComponent<MeshFilter> ();
			setMesh3 = type3Mesh;
			break;	
		default:
			MeshFilter setMeshDefault = gameObject.GetComponent<MeshFilter> ();
			setMeshDefault = type3Mesh;
			break;
		}
    }

	// Update function
	public void UpdateVoxel () {
		// Set the future state
		state = futureState;
        // If voxel is alive update age
        if (state == 1)
        {
            age++;
        }
        // If voxel is death disable the game object mesh renderer and set age to zero
        if (state == 0)
        {
            age = 0;
        }
    }


    /// <summary>
    /// Setters and Getters - Allow us to access and set private variables
    /// </summary>
    /// <param name="_state"></param>
	// Set the state of the voxel
	public void SetState(int _state){
		state = _state;
	}

	// Set the future state of the voxel
	public void SetFutureState(int _futureState){
		futureState = _futureState;
	}

    // Get the age of the voxel
	public void SetAge(int _age){
		age = _age;
	}

	// Get the state of the voxel
	public int GetState(){
		return state;
	}

	// Get the age of the voxel
	public int GetAge(){
		return age;
	}

    //Set 3d Moores Neighborhood Density 
    public void setDensity3dMO(int _density3dMO)
    {
        density3dMO = _density3dMO;
    }
    //Get 3d Moores Neighborhood Density 
    public int getDensity3dMO()
    {
        return density3dMO;
    }

    //Set 3d Von Neumann Neighborhood Density 
    public void setDensity3dVN(int _density3dVN)
    {
        density3dVN = _density3dVN;
    }
    //Get 3d Von Neumann Neighborhood Density 
    public int getDensity3dVN()
    {
        return density3dVN;
    }



    /// <summary>
    /// VOXEL NEIGHBORHOOD GETTERS/SETTERS
    /// </summary>
    /// 

    //MOORES NEIGHBORS (26 PER VOXEL)
    public void setNeighbors3dMO(Voxel[] _setNeighbors3dMO)
    {
        neighbors3dMO = _setNeighbors3dMO;
    }

    public Voxel[] getNeighbors3dMO()
    {
        return neighbors3dMO;
    }

    //VON NEUMANN NEIGHBORS (6 PER VOXEL)
    public void setNeighbors3dVN(Voxel[] _setNeighbors3dVN)
    {
        neighbors3dVN = _setNeighbors3dVN;
    }

    public Voxel[] getNeighbors3dVN()
    {
        return neighbors3dVN;
    }


    //voxel above this
    public void setVoxelAbove(Voxel _voxelAbove)
    {
        voxelAbove = _voxelAbove;
    }

    public Voxel getVoxelAbove()
    {
        return voxelAbove;
    }

    //voxel below this
    public void setVoxelBelow(Voxel _voxelBelow)
    {
        voxelBelow = _voxelBelow;
    }

    public Voxel getVoxelBelow()
    {
        return voxelBelow;
    }

    //voxel right of this
    public void setVoxelRight(Voxel _voxelRight)
    {
        voxelRight = _voxelRight;
    }

    public Voxel getVoxelRight()
    {
        return voxelRight;
    }

    //voxel left of this
    public void setVoxelLeft(Voxel _voxelLeft)
    {
        voxelLeft = _voxelLeft;
    }

    public Voxel getVoxelLeft()
    {
        return voxelLeft;
    }

    //voxel in front of this
    public void setVoxelFront(Voxel _voxelFront)
    {
        voxelFront = _voxelFront;
    }

    public Voxel getVoxelFront()
    {
        return voxelFront;
    }

    //voxel in back of this
    public void setVoxelBack(Voxel _voxelBack)
    {
        voxelBack = _voxelBack;
    }

    public Voxel getVoxelBack()
    {
        return voxelBack;
    }

    // Update the voxel display
    public void VoxelDisplay()
    {
        if (state == 1)
        {
            // Set Color
            Color col = new Color(1f, 0.9f, 0.48f, 1);
            props.SetColor("_Color", col);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    public void VoxelDisplay(float  _r, float  _g, float  _b)
    {
        if (state == 1)
        {
            // Set Color
            Color col = new Color(_r, _g, _b, 1);
            props.SetColor("_Color", col);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Age
    /// </summary>
    /// <param name="_maxAge"></param>
    public void VoxelDisplayAge(int _maxAge)
    {
        if (state == 1)
        {
            // Remap the age value relative to maxage to range of 0,1
            float mappedvalue = Remap(age, 0, _maxAge, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(0.2f, 0.2f, 0.2f, 1);
            Color color2 = new Color(1, 1, 1, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }



    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dMO(int _maxdensity3dMO)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMO, 0, _maxdensity3dMO, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(0.7f, 0, 0.2f, 1);
            Color color2 = new Color(0.1f, 0.7f, 0.3f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dVN(int _maxdensity3dVN)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMO, 0, _maxdensity3dVN, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 0f,1f, 1);
            Color color2 = new Color(0.15f, 0.4f, 0.9f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    public void VoxelDisplayLayerDensity(float _layerdensity, float _minlayerdensity, float _maxlayerdensity)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(_layerdensity, 0, _maxlayerdensity, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(0.2f, 0.1f, 0.8f, 1);
            Color color2 = new Color(0.8f, 0.92f, 0.016f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    // Remap numbers - used here for getting a gradient of color across a range
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
