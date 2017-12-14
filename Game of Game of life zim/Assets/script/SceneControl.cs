using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour {

    public void ReloadScene(int index)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DynamicGI.UpdateEnvironment();
       
    }

}
