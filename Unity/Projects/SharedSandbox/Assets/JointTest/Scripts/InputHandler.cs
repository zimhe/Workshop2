using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */

/// <summary>
/// 
/// </summary>
public class InputHandler : MonoBehaviour
{
    public GridCreator GridCreator;

    
    /// <summary>
    /// 
    /// </summary>
    public void OnResetClick()
    {
        GridCreator.ResetGrid();
    }
}
