using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;


public class SeedImageToogle : MonoBehaviour
{

    //boolean toogle seedimage
    private bool SeedImagePreview = false;

    private bool Displaying = false;

    GameObject[,] SeedImageGrid;
   public Texture2D SeedImagePre;
    public GameObject SeedImagePrefab;


    private int previewWidth;
    private int previewLength;

     int SeedImageLevel = 0;

    // Use this for initialization
    void Start()
    {
        //SeedImagePre = GetComponent<Environment>().seedImageInUse;
        previewWidth = SeedImagePre.width;
        previewLength = SeedImagePre.height;

        
        
    }

    public void BuildSeedImage()
    {
        SeedImageGrid = new GameObject [previewWidth, previewLength];
        for (int i = 0; i < previewWidth; i++)
        {
            for (int j = 0; j < previewLength; j++)
            {
                Vector3 SeedImageVoxelPos = new Vector3(i, SeedImageLevel , j);
                Quaternion SeedImageVoxelRot = Quaternion.identity;
                //create the game object of the voxel
                GameObject SeedImageVoxel = Instantiate(SeedImagePrefab , SeedImageVoxelPos, SeedImageVoxelRot);

                SeedImageVoxel.GetComponent<Voxel>().SetupVoxel(i, j, SeedImageLevel, 1);

                SeedImageGrid[i, j] = SeedImageVoxel;

                int SeedImageState =(int)SeedImagePre.GetPixel(i, j).grayscale;
                SeedImageGrid[i, j].GetComponent<Voxel>().SetState(SeedImageState);
               /* if (SeedImageGrid[i, j].GetComponent<Voxel>().GetState() == 0)
                {
                    Destroy(SeedImageGrid[i, j]);
                }*/
                //SeedImageGrid[i, j].GetComponent<Voxel>().VoxelDisplay();
            }
        }
    }


    // Update is called once per frame
        void Update()
        {



            if (Input.GetKeyDown(KeyCode.S))
            {
                if (SeedImagePreview == false)
                {
                    SeedImagePreview = true;
                    if (Displaying == false)
                    {
                        BuildSeedImage();
                    }
            }
                else if
                    (SeedImagePreview == true)
                {
                    SeedImagePreview = false;
                }
            }



            for (int i = 0; i < previewWidth; i++)
            {
                    for (int j = 0; j <previewLength; j++)
                    {
                        if (SeedImagePreview == true)
                        {
                            Displaying = true;
                            SeedImageGrid[i, j].GetComponent<Voxel>().VoxelDisplay();
                        }

                        if (SeedImagePreview == false)
                        {
                            if ( Displaying == true)
                            { 
                            Destroy(SeedImageGrid[i, j]);
                            }
                            if (i +1== previewWidth && j +1== previewLength)
                            {
                                Displaying = false;
                            }
                            
                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            SeedImageGrid[i, j].gameObject.transform.Translate(Vector3.up);
                        }
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            SeedImageGrid[i, j].gameObject.transform.Translate(Vector3.down);
                        }
                    }
            }
             if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SeedImageLevel += 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SeedImageLevel -= 1;
            }
        }

}


    

