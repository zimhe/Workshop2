using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class Environment : MonoBehaviour
{

    // VARIABLES

    // Texture to be used as start of CA input
    Texture2D seedImageInUse;

    GOLRule RuleInUse;

    private float maxlayerdensity = 0;
    private float minlayerdensity = 10000;

    public Texture2D[] seedImageStore;
    private GOLRule[] ruleStore = new GOLRule[13];
    private float[] conditionDensity = new float[12];
    private List<string> conditionDensityOptions = new List<string>();
    private List<string> ageOptions = new List<string>();


    //dropdownlist
    public Dropdown SeedImageDropdown;

    public Dropdown RuleDropdown;

    public Dropdown toLayerDensity;

    public Dropdown[] ConditionRule;

    public Dropdown[] FrameDropdowns;

    public Dropdown[] DensityDropdowns;

    public Dropdown AgeDropdown;

    public Slider TimeEndSlider;
    public Slider MaxAgeSlider;
    public Slider ShowLayerSection;

    public Slider ShowLayerSection2;
    public Toggle ToggleHighlight;

    public Toggle[] ConditionsToggles;






    public RawImage SeedImagepreview;

    private int vizmode = 0;

    private int LayerCount = 0;

    private int Cuttingindex;

    

    float DensityTotal;





    private List<string> seedImageOptions = new List<string> {"None Image"};

    private List<string> rulesOptions = new List<string> {"None Rule"};

    private List<string> rulesOptionsCON = new List<string> {"None"};

    private List<string> TotalLayerOptions = new List<string>();

    private List<GameObject> VoxelGridList = new List<GameObject>();

    private List<string> Frames = new List<string>();

    public Text seedImageText;

    public Text rulesText;

    public Text TimeEndText;

    public Text MaxAgeText;

    public Text Layerdens;

    public Text TotalDensityValue;

    public Text MaxLayerValue;

    public Text MinLayerValue;

    // Number of frames to run which is also the height of the CA
    private int timeEnd;

    private int MaxAgeAll;
    int currentFrame = 0;

    //variables for size of the 3d grid
    int width;

    int length;
    int height;

    // Array for storing voxels
    GameObject[,,] voxelGrid;

    private int[,,] SavedVoxelState;

    // Reference to the voxel we are using
    public GameObject voxelPrefab;

    // Spacing between voxels
    float spacing = 1.0f;

    //Layer Densities
    int totalAliveCells = 0;

    float layerdensity = 0;
    float[] layerDensities;

    //Max Age
    int maxAge = 0;

    //Max Densities
    int maxDensity3dMO = 0;

    int maxDensity3dVN = 0;

    // Setup Different Game of Life Rules
    GOLRule deathrule = new GOLRule();

    GOLRule rule1 = new GOLRule();
    GOLRule rule2 = new GOLRule();
    GOLRule rule3 = new GOLRule();
    GOLRule rule4 = new GOLRule();
    GOLRule rule5 = new GOLRule();
    GOLRule rule6 = new GOLRule();
    GOLRule rule7 = new GOLRule();
    GOLRule rule8 = new GOLRule();
    GOLRule rule9 = new GOLRule();
    GOLRule rule10 = new GOLRule();
    GOLRule rule11 = new GOLRule();
    GOLRule rule12 = new GOLRule();
    GOLRule rule13 = new GOLRule();
   



    private bool seedSelected = false;

    private bool ruleSelected = false;

    private bool startNew = false;

    private bool clearToBuild = false;

    private bool gridDone = false;

    private bool HighLight = false;

    private bool ToShowSection = false;


    //boolean switches
    //toggles pausing the game
    bool pause = false;

    // FUNCTIONS

    public void vizmode0()
    {
        vizmode = 0;
        ShowLayerSection.gameObject.active = false;
        ShowLayerSection2.gameObject.active = false;
    }

    public void vizmode1()
    {
        vizmode = 1;
        ShowLayerSection.gameObject.active = true;
        ShowLayerSection2.gameObject.active = true;
    }

    public void vizmode2()
    {
        vizmode = 2;
        ShowLayerSection.gameObject.active = true;
        ShowLayerSection2.gameObject.active = true;
    }

    public void vizmode3()
    {
        vizmode = 3;
        ShowLayerSection.gameObject.active = true;
        ShowLayerSection2.gameObject.active = true;
    }

    public void vizmode4()
    {
        vizmode = 4;
        ShowLayerSection.gameObject.active = true;
        ShowLayerSection2.gameObject.active = true;
    }
    public void vizmode5()
    {
        vizmode = 5;
        ShowLayerSection.gameObject.active = true;
        ShowLayerSection2.gameObject.active = true;
    }

    public void resetVoxelGrid()
    {
        vizmode0();
        currentFrame = 0;
        LayerCount = 0;
        startNew = true;
        SelectSeed();
        SelectRule();
        minlayerdensity = 10000;
        maxlayerdensity = 0;
        pause = false;


    }

    public void restoreRulesNSeeds()
    {
        SeedImageDropdown.value = 0;
        RuleDropdown.value = 0;
    }

    public void removeLastModel()
    {
        toLayerDensity.value = 0;
        if (VoxelGridList.Count > 0)
        {
            foreach (GameObject voxel in voxelGrid)
            {
                Destroy(voxel);
                VoxelGridList.Remove(voxel);
            }
        }
            
        gridDone = false;

        toLayerDensity.options.Clear();
        for (int i = 1; i <= TotalLayerOptions.Count; i++)
        {
            TotalLayerOptions.Clear();
        }


        /*for (int i = 0, x = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++, x++)
                {
                    Destroy(voxelGrid[i, j, k]);
                    VoxelGridList.Remove(voxelGrid [i,j,k]);
                }
            }
        }*/
    }

    public void SelectSeed()
    {
        int index1 = SeedImageDropdown.value;

        if (index1 > 0)
        {
            seedImageInUse = seedImageStore[index1 - 1];
            width = seedImageInUse.width;
            length = seedImageInUse.height;
            seedSelected = true;
        }
        else
        {
            seedSelected = false;
        }
    }

    public void SelectSeed(int ImageIndex)
    {
       SeedImageDropdown.value=ImageIndex ;

        if (ImageIndex > 0)
        {


            seedImageInUse = seedImageStore[ImageIndex  - 1];
            width = seedImageInUse.width;
            length = seedImageInUse.height;
            seedSelected = true;
        }
        else
        {
            seedSelected = false;
        }
    }

    public void SelectRule()
    {
        int index2 = RuleDropdown.value;

        if (index2 > 0)
        {
            ruleSelected = true;
            RuleInUse = ruleStore[index2 - 1];
        }
        else
        {
            ruleSelected = false;
        }

    }

    public void SelectRule(int RuleIndex)
    {
        RuleDropdown.value = RuleIndex;

        if (RuleIndex  > 0)
        {
            ruleSelected = true;
            rulesText.text = rulesOptions[RuleIndex ] + " Selected";
            RuleInUse = ruleStore[RuleIndex - 1];
        }
        else
        {
            ruleSelected = false;
        }

    }

    // Use this for initialization
    void Start()
    {
        // Read the image width and height

        //VoxelGridList.Add(test );


        ShowLayerSection.gameObject .active  = false ;
        ShowLayerSection2.gameObject .active  = false ;

        populateSeedImageList();

        populateFrameNAgeCON();

        //toLayerDensity.AddOptions(SelecetLayer);



        //Setup GOL Rules
        rule1.setupRule(1, 2, 2, 2); 
        rule2.setupRule(1, 2, 3, 3); 
        rule3.setupRule(1, 2, 3, 4); 
        rule4.setupRule(1, 3, 3, 3); 
        rule5.setupRule(1, 3, 3, 6); 
        rule6.setupRule(2, 3, 3, 3); 
        rule7.setupRule(2, 3, 3, 4); 
        rule8.setupRule(3, 3, 2, 2); 
        rule9.setupRule(3, 4, 1, 1); 
        rule10.setupRule(3, 4, 3, 4); 
        rule11.setupRule(3, 6, 3, 3); 
        rule12.setupRule(4, 5, 2, 2);
        rule13.setupRule(5, 5, 1, 1);
        


        deathrule.setupRule(0, 0, 0, 0);
        //store rules in array
        ruleStore[0] = rule1;
        ruleStore[1] = rule2;
        ruleStore[2] = rule3;
        ruleStore[3] = rule4;
        ruleStore[4] = rule5;
        ruleStore[5] = rule6;
        ruleStore[6] = rule7;
        ruleStore[7] = rule8;
        ruleStore[8] = rule9;
        ruleStore[9] = rule10;
        ruleStore[10] = rule11;
        ruleStore[11] = rule12;
        ruleStore[12] = rule13;
      




        populateRuleList();



        //Layer Densities
        //layerDensities = new float[timeEnd];



        // Create a new CA grid


    }

    void populateSeedImageList()
    {
        // Read value from somewhere
        {
            for (int i = 0; i < seedImageStore.Length; i++)
            {
                seedImageStore[i].name = "Image " + (i + 1);
                seedImageOptions.Add(seedImageStore[i].name);
            }
            SeedImageDropdown.AddOptions(seedImageOptions);


        }
    }

    void populateRuleList()
    {
        for (int i = 0; i < ruleStore.Length; i++)
        {
            rulesOptions.Add("Rule " + (i + 1) + " : " + ruleStore[i].getInstruction(0) + ", " +
                             ruleStore[i].getInstruction(1) + ", " + ruleStore[i].getInstruction(2) + ", " +
                             ruleStore[i].getInstruction(3));
        }
        RuleDropdown.AddOptions(rulesOptions);

        for (int i = 0; i < ruleStore.Length; i++)
        {
            rulesOptionsCON.Add("Rule" + (i + 1));
        }
        for (int j = 0; j < ConditionRule.Length; j++)
        {
            ConditionRule[j].AddOptions(rulesOptionsCON);
        }

        for (int i = 0; i < 4; i++)
        {
            conditionDensity[i] = 0.01f * (i + 1);
            conditionDensityOptions.Add(conditionDensity[i].ToString());
        }

        for (int i = 4; i < 12; i++)
        {
            conditionDensity[i] = 0.05f * (i-3);
            conditionDensityOptions.Add(conditionDensity[i].ToString());
        }

        for (int i = 0; i < DensityDropdowns.Length; i++)
        {
            DensityDropdowns[i].AddOptions(conditionDensityOptions);
        }

    }

    void populateLayerDens()
    {
        TotalLayerOptions.Add("DENSITY OF LAYERS");
        for (int i = 1; i < LayerCount; i++)
        {
            TotalLayerOptions.Add("LAYER " + i + " | " + layerDensities[i]);
        }
        toLayerDensity.AddOptions(TotalLayerOptions);

    }

    void setNewTimeAge()
    {
        timeEnd = (int) TimeEndSlider.value;
        height = timeEnd;
        MaxAgeAll = (int) MaxAgeSlider.value;
        layerDensities = new float[timeEnd];

    }


    // Update is called once per frame
    void Update()
    {
        Cuttingindex = (int)ShowLayerSection.value;
     
        SeedImagepreview.texture = seedImageInUse;

        TimeEndText.text = TimeEndSlider.value.ToString();
        MaxAgeText.text =  MaxAgeSlider.value.ToString( );

        if (currentFrame > 0)
        {
            startNew = false;
        }

        if (VoxelGridList.Count > 0)
        {
            clearToBuild = false;
        }
        if (VoxelGridList.Count == 0)
        {
            clearToBuild = true;
        }
        if (startNew == true && clearToBuild == false)
        {
            removeLastModel();

        }
        if (startNew == true && clearToBuild == true && seedSelected == true && ruleSelected == true)
        {
            CreateGrid();
            SetupNeighbors3d();
        }

        if (startNew == true)
        {
            setNewTimeAge();
        }

        // Calculate the CA state, save the new state, display the CA and increment time frame
        if (gridDone == true)
        {
            if (currentFrame < timeEnd)
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

                    //Update 3d Densities
                    updateDensities3d();



                    DensityStructure();




                    // Increment the current frame count
                    currentFrame++;


                }

            }
            HighlightToggle();

            


            //Change vizmode
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 1; k < height; k++)
                    {
                       
                        if (vizmode == 0)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
                            if (HighLight == true && toLayerDensity.value > 0&&k!=ShowLayerSection .value )
                            {
                                voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1f, 0.9f, 0.48f);
                            }
                        }

                        if (vizmode == 1)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayAge(maxAge);
                            if (HighLight == true && toLayerDensity.value > 0 && k != ShowLayerSection.value)
                            {
                                voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1, 0.3f, 0);
                            }

                            if (k > (int)ShowLayerSection.value || j > (int)ShowLayerSection2.value)
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().renderer.enabled = false;
                            }
                        }

                        if (vizmode == 3)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dMO(maxDensity3dMO);
                            if (HighLight == true && toLayerDensity.value > 0 && k != ShowLayerSection.value)
                            {
                                voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1, 0.3f, 0);
                            }


                            if (k > (int)ShowLayerSection.value || j > (int)ShowLayerSection2.value)
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().renderer.enabled = false;
                            }
                        }
                        if (vizmode == 2)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dVN(maxDensity3dVN);
                            if (HighLight == true && toLayerDensity.value > 0 && k != ShowLayerSection.value)
                            {
                                voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1, 0.3f, 0);
                            }


                            if (k > (int)ShowLayerSection.value || j > (int)ShowLayerSection2.value)
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().renderer.enabled = false;
                            }
                        }
                        if (vizmode == 4)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>()
                                .VoxelDisplayLayerDensity(layerDensities[k], minlayerdensity, maxlayerdensity);
                            if (HighLight == true && toLayerDensity.value > 0 && k != ShowLayerSection.value)
                            {
                                voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1, 0.3f, 0);
                            }


                            if (k > (int)ShowLayerSection.value || j > (int)ShowLayerSection2.value)
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().renderer.enabled = false;
                            }


                        }
                        if (vizmode == 5)
                        {
                           

                            if (k > (int)ShowLayerSection.value || j>(int)ShowLayerSection2 .value )
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().renderer.enabled = false;
                            }
                      
                            if (k < (int) ShowLayerSection.value && j < (int)ShowLayerSection2.value)
                            {
                                voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay(1, 1, 1);
                                if (k < (int) ShowLayerSection.value && j < (int) ShowLayerSection2.value)
                                {
                                    voxelGrid[i, j, (int)ShowLayerSection.value].GetComponent<Voxel>().VoxelDisplay(1, 0, 0.5f);
                                    voxelGrid[i, (int)ShowLayerSection2.value, k].GetComponent<Voxel>().VoxelDisplay(0.15f, 0.6f, 0.9f);
                                    voxelGrid[i, (int)ShowLayerSection2.value, (int)ShowLayerSection.value].GetComponent<Voxel>().VoxelDisplay(0.9f, 0, 0.2f);
                                }
                                if (HighLight == true && toLayerDensity.value > 0 && k != ShowLayerSection.value)
                                {
                                    voxelGrid[i, j, toLayerDensity.value].GetComponent<Voxel>().VoxelDisplay(1, 0.3f, 0);
                                }
                            }

                        }
                       

                    }
                }
            }

        }


        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        /*  if (Input.GetKeyDown(KeyCode.Space))
          {
              if (gameObject.GetComponent<ModelDisplay>() == null)
              {
                  gameObject.AddComponent<ModelDisplay>();
              }
              else
              {
                  Destroy(gameObject.GetComponent<ModelDisplay>());
              }
          }*/
        if (currentFrame == timeEnd - 1)
        {
            currentFrame++;
            if (currentFrame == timeEnd)
            {
                populateLayerDens();
                LayerSectionSetup();
            }

        }

       
        ShowLayerDens();
        // Export the voxel aggregation
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (GameObject currentGameObject in voxelGrid)
            {
                Voxel currentVoxel = currentGameObject.GetComponent<Voxel>();
                if (currentVoxel.GetState() == 0)
                {
                    Destroy(currentGameObject);
                }
            }
            print("Ready to export!");
        }
    }

    public void ShowLayerDens()
    {
        int index = toLayerDensity.value;
        if (index > 0)
        {
            Layerdens.text = layerDensities[index].ToString();
        }

        MinLayerValue.text = minlayerdensity.ToString();
        MaxLayerValue.text = maxlayerdensity.ToString();
        TotalDensityValue.text = DensityTotal.ToString();

        if (gridDone == false)
        {
            string Value = "- - -";
            MinLayerValue.text = Value;
            MaxLayerValue.text = Value;
            TotalDensityValue.text = Value;
            Layerdens.text = Value;
        }

    }

    public void HighlightToggle()
    {
        if (ToggleHighlight.isOn)
        {
            HighLight = true;
        }
        else
        {
            HighLight = false;
        }
    }

    public void Pause()
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

    public void RandomGrow()
    {
        int indexSeed = Random.Range(0, seedImageStore.Length - 1);
        int indexRule = Random.Range(0, ruleStore.Length - 1);

        SeedImageDropdown.value = indexSeed + 1;
        RuleDropdown.value = indexRule + 1;

    }

    void DensityStructure()
    {
        int alivetotalcount = 0;


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 1; k < timeEnd; k++)
                {
                    GameObject currentVoxelObj = voxelGrid[i, j, k];
                    
                    //UPDATE THE VON NEUMANN NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    int temVoxelStare = currentVoxelObj.GetComponent<Voxel>().GetState();

                    if (temVoxelStare == 1)
                    {
                        alivetotalcount++;
                    }
                }
            }
        }

        DensityTotal = (float) alivetotalcount / voxelGrid.Length;
        }

    // Create grid function
    void CreateGrid()
    {




        // Allocate space in memory for the array
        voxelGrid = new GameObject[width, length, height];
        // Populate the array with voxels from a base image
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    // Create values for the transform of the new voxel
                    Vector3 currentVoxelPos = new Vector3(i * spacing, k * spacing, j * spacing);
                    Quaternion currentVoxelRot = Quaternion.identity;
                    //create the game object of the voxel
                    GameObject currentVoxelObj = Instantiate(voxelPrefab, currentVoxelPos, currentVoxelRot);
                    //run the setupVoxel() function inside the 'Voxel' component of the voxelPrefab
                    //this sets up the instance of Voxel class inside the Voxel game object
                    currentVoxelObj.GetComponent<Voxel>().SetupVoxel(i, j, k, 1);

                    // Set the state of the voxels
                    if (k == 0)
                    {
                        // Create a new state based on the input image

                        float t = seedImageInUse.GetPixel(i, j).grayscale;

                        /*
                         // white -> alive
                         if (t > 0.5f)
                             currentVoxelObj.GetComponent<Voxel>().SetState(1);
                         else
                             currentVoxelObj.GetComponent<Voxel>().SetState(0);
                       */

                        // or

                        // black - > alive
                        if (t > 0.8f)
                            currentVoxelObj.GetComponent<Voxel>().SetState(0);
                        else
                            currentVoxelObj.GetComponent<Voxel>().SetState(1);

                        /*
                        // gray - > alive
                        if (t > 0.49 && t < 0.51)
                            currentVoxelObj.GetComponent<Voxel>().SetState(0);
                        else
                            currentVoxelObj.GetComponent<Voxel>().SetState(1);
                        */
                    }
                    else
                    {
                        // Set the state to death
                        currentVoxelObj.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxelObj.GetComponent<Voxel>().SetState(0);
                    }
                    // Save the current voxel in the voxelGrid array
                    voxelGrid[i, j, k] = currentVoxelObj;
                    // Attach the new voxel to the grid game object
                    currentVoxelObj.transform.parent = gameObject.transform;

                    VoxelGridList.Add(voxelGrid[i, j, k]);
                    gridDone = true;

                }
            }
        }
    }

    // Calculate CA function
    void CalculateCA()
    {
        // Go over all the voxels stored in the voxels array


        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                int aliveNeighbours = 0;

                // Calculate how many alive neighbours are around the current voxel
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        GameObject currentNeigbour = voxelGrid[i + x, j + y, 0];
                        int currentNeigbourState = currentNeigbour.GetComponent<Voxel>().GetState();
                        aliveNeighbours += currentNeigbourState;
                    }
                }
                aliveNeighbours -= currentVoxelState;


                if (ConditionsToggles[0].isOn)
                {
                    RuleChangeOnFrm1();
                }

                if (ConditionsToggles[1].isOn)
                {
                    RuleChangeOnFrm2();
                }

                if (ConditionsToggles[2].isOn)
                {
                    RuleChangeOnDens1();
                }
                if (ConditionsToggles[3].isOn)
                {
                    RuleChangeOnDens2();
                }






                //get the instructions
                int inst0 = RuleInUse.getInstruction(0);
                int inst1 = RuleInUse.getInstruction(1);
                int inst2 = RuleInUse.getInstruction(2);
                int inst3 = RuleInUse.getInstruction(3);

                // Rule Set 1: for voxels that are alive
                if (currentVoxelState == 1)
                {
                    // If there are less than two neighbours I am going to die
                    if (aliveNeighbours < inst0)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                    }
                    // If there are two or three neighbours alive I am going to stay alive
                    if (aliveNeighbours >= inst0 && aliveNeighbours <= inst1)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                    }
                    // If there are more than three neighbours I am going to die
                    if (aliveNeighbours > inst1)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                    }
                }
                // Rule Set 2: for voxels that are death
                if (currentVoxelState == 0)
                {
                    // If there are exactly three alive neighbours I will become alive
                    if (aliveNeighbours >= inst2 && aliveNeighbours <= inst3)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                    }
                }

                //age - here is an example of a condition where the cell is "killed" if its age is above a threshhold
                // in this case if this rule is put here after the Game of Life rules just above it, it would override 
                // the game of lie conditions if this condition was true

                if (currentVoxelObj.GetComponent<Voxel>().GetAge() > MaxAgeAll)
                {
                    currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                }
                if (ConditionsToggles[4].isOn)
                {
                    if (currentVoxelObj.GetComponent<Voxel>().GetAge() > AgeDropdown.value + 1)
                    {
                        RuleChangeOnAge();
                    }


                }

            }
        }
    }

    // Save the CA states - this is run after the future state of all cells is calculated to update/save
    //current state on the current level
    void SaveCA()
    {
        
        //counter stores the number of live cells on this level and is incremented below 
        //in the for loop for each cell with a state of 1
        totalAliveCells = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];
                
                savedVoxel.GetComponent<Voxel>().SetState(currentVoxelState);
                
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1)
                {
                    int currentVoxelAge = currentVoxelObj.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);
                    totalAliveCells++;
                    //track oldest voxels
                    if (currentVoxelAge > maxAge)
                    {
                        maxAge = currentVoxelAge;
                    }
                }
            }
        }

        float totalcells = length * width;
        layerdensity = totalAliveCells / totalcells;

        layerDensities[currentFrame] = layerdensity;
        // print(layerDensities[currentFrame]);
        //this stores the density of live cells for each entire layer of cells(each level)	 

        if (layerdensity > 0)
        {
            LayerCount++;
            if (layerdensity > maxlayerdensity)
            {
                maxlayerdensity = layerdensity;
            }

            if (layerdensity < minlayerdensity)
            {
                minlayerdensity = layerdensity;
            }
        }
    }

    /// <summary>
    /// SETUP MOORES & VON NEUMANN 3D NEIGHBORS
    /// </summary>
    void SetupNeighbors3d()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < height - 1; k++)
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
                                if ((i + m >= 0) && (i + m < width) && (j + n >= 0) && (j + n < length) &&
                                    (k + p >= 0) && (k + p < height))
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

    /// <summary>
    /// Update 3d Densities for Each Voxel
    /// </summary>
    void updateDensities3d()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < currentFrame; k++)
                {
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    //UPDATE THE VON NEUMANN NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsVN = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dVN();
                    int alivecount = 0;
                    foreach (Voxel vox in tempNeighborsVN)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dVN(alivecount);
                    if (alivecount > maxDensity3dVN)
                    {
                        maxDensity3dVN = alivecount;
                    }

                    //UPDATE THE MOORES NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsMO = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dMO();
                    alivecount = 0;
                    foreach (Voxel vox in tempNeighborsMO)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dMO(alivecount);
                    if (alivecount > maxDensity3dMO)
                    {
                        maxDensity3dMO = alivecount;
                    }
                }
            }
        }
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


    public void RuleChangeOnFrm1()
    {
        if (currentFrame > FrameDropdowns[0].value && ConditionRule[0].value > 0)
        {
            RuleInUse = ruleStore[ConditionRule[0].value - 1];
        }
    }
    public void ShortCutRuleChangeOnFrm1(int _x, int _y)
    {
        if ( _y > 0)
        {
            ConditionsToggles[0].isOn = true;
            FrameDropdowns[0].value = _x-5;
            ConditionRule[0].value = _y;
        }
        else
        {
            ConditionsToggles[0].isOn = false;
        }
    }


    public void RuleChangeOnFrm2()
    {
        if (currentFrame > FrameDropdowns[1].value&& ConditionRule[1].value > 0)
        {
            RuleInUse = ruleStore[ConditionRule[1].value - 1];
        }
    }
    public void ShortCutRuleChangeOnFrm2(int _x, int _y)
    {
        if ( _y > 0)
        {
            ConditionsToggles[1].isOn = true;
            FrameDropdowns[1].value = _x-5;
            ConditionRule[1].value = _y;
        }
        else
        {
            ConditionsToggles[1].isOn = false;
        }
    }

    public void RuleChangeOnDens1()
    {
        int index = DensityDropdowns[0].value;
        int index2 = ConditionRule[2].value;
        if (layerdensity < conditionDensity[index] && currentFrame > FrameDropdowns [0].value && ConditionRule[2].value > 0)
        {
            RuleInUse = ruleStore[index2-1];
        }
    }
    public void ShortCutRuleChangeOnDens1(int _x, int _y)
    {
        if ( _y > 0)
        {
            ConditionsToggles[2].isOn = true;
            DensityDropdowns[0].value = _x;
            ConditionRule[2].value = _y;
        }
        else
        {
            ConditionsToggles[2].isOn = false;
        }
    }

    public void RuleChangeOnDens2()
    {
        int index = DensityDropdowns[1].value;
        int index2 = ConditionRule[3].value;
        if (layerdensity > conditionDensity[index] && currentFrame > FrameDropdowns [0].value && ConditionRule [3].value >0)
        {
            RuleInUse = ruleStore[index2-1];
        }
    }
    public void ShortCutRuleChangeOnDens2(int _x, int _y)
    {
        if ( _y > 0)
        {
            ConditionsToggles[3].isOn = true;
            DensityDropdowns[1].value = _x;
            ConditionRule[3].value = _y;
        }
        else
        {
            ConditionsToggles[3].isOn = false;
        }
    }

    public void RuleChangeOnAge()
    {
        if (ConditionRule[4].value > 0)
        {
            RuleInUse = ruleStore[ConditionRule[4].value - 1];
        }
      
    }
    public void ShortCutRuleChangeOnAge(int _x, int _y)
    {
        if ( _y > 0)
        {
            ConditionsToggles[4].isOn = true;
            AgeDropdown.value = _x-1;
            ConditionRule[4].value = _y;
        }
        else
        {
            ConditionsToggles[4].isOn = false;
        }
    }

    public void MakeSomthing(int Seed, int Rule, int FRM1, int CR1, int FRM2, int CR2, int Dens3, int CR3, int Dens4,
        int CR4, int Age5, int CR5,int TMED, int MXAG)
    {
        vizmode0();
        currentFrame = 0;
        LayerCount = 0;
        startNew = true;
        minlayerdensity = 10000;
        maxlayerdensity = 0;

        SelectSeed(Seed);
        SelectRule(Rule);
        ShortCutRuleChangeOnFrm1(FRM1, CR1);
        ShortCutRuleChangeOnFrm2(FRM2, CR2);
        ShortCutRuleChangeOnDens1(Dens3, CR3);
        ShortCutRuleChangeOnDens2(Dens4, CR4);
        ShortCutRuleChangeOnAge(Age5, CR5);
        TimeEndSlider.value = TMED;
        MaxAgeSlider.value = MXAG ;
    }

    public void MakeATree()
    {
        MakeSomthing(3, 6, 20, 11, 38, 6, 0, 0, 0, 0, 0, 0,65,5);
    }

    public void MakeAFlower()
    {
        MakeSomthing(8, 3, 20, 6, 40, 8, 0, 8, 4, 6, 0, 0,65,5);
    }

    public void MakeABouquet()
    {
        MakeSomthing(8, 3, 20, 6, 40, 7, 0, 0, 0, 0, 0, 0, 60, 5);
    }

    public void MakeAParterre()
    {
        MakeSomthing(9, 8, 25, 6, 45, 8, 4, 4, 5, 10, 0, 0, 60, 5);
    }

    public void MakeABush()
    {
        MakeSomthing(2, 8, 20, 7, 40, 3, 5, 7, 6, 6, 0, 0, 60, 5);
    }

    public void MakeAPerfectTalbe()
    {
        MakeSomthing(2, 12, 20, 7, 40, 13, 5, 4, 7, 11, 0, 0, 60, 5);
    }

    public void MakeALovelyTable()
    {
        MakeSomthing(3, 6, 20, 3, 40, 1, 0, 0, 0, 0, 0, 0, 65, 5);
    }

    public void MakeAClassicalTable()
    {
        MakeSomthing(9, 8, 25, 6, 45, 9, 4, 6, 5, 8, 0, 0, 60, 5);
    }

    public void MakeABrokenTable()
    {
        MakeSomthing(2, 12, 20, 4, 40, 11, 5, 10, 7, 6, 2, 7, 60, 5);
    }

    public void MakeATripod()
    {
        MakeSomthing(2, 12, 20, 7, 40, 11, 5, 4, 7, 3, 0, 0, 60, 5);
    }



    void populateFrameNAgeCON()
    {
        for (int i = (int) TimeEndSlider.minValue; i <= TimeEndSlider.maxValue; i++)
        {
            Frames.Add(i.ToString());
        }
        for (int i = 0; i < FrameDropdowns.Length; i++)
        {
            FrameDropdowns[i].AddOptions(Frames);
        }
        for (int i = (int) MaxAgeSlider.minValue; i <= MaxAgeSlider.maxValue; i++)
        {
            ageOptions.Add(i.ToString());
        }
        AgeDropdown.AddOptions(ageOptions);
    }

    void LayerSectionSetup()
    {
        if (currentFrame == timeEnd)
        {
            ShowLayerSection.maxValue = LayerCount-1;
            ShowLayerSection2.maxValue = length-1;
            ShowLayerSection.value = ShowLayerSection.maxValue;
            ShowLayerSection2.value = ShowLayerSection2.maxValue;
        }
    }
    public void ShowSectionDens(float SectionIndex)
    {
        toLayerDensity.value = (int)SectionIndex;
    }



    public int  getVizmode()
    {
        return vizmode;
    }
}
