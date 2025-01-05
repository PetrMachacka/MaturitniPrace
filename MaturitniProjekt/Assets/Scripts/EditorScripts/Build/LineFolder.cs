using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class LineFolder : MonoBehaviour
{
    public GameObject Folder;
    private void Update(){
        ToggleLineFolder();
    }
    private void ToggleLineFolder(){
        if(BuildManager.buildMode == BuildModes.logic && !Folder.activeSelf){
            Folder.SetActive(true);
        }
        else if(BuildManager.buildMode != BuildModes.logic && Folder.activeSelf){
            Folder.SetActive(false);
        }
    }
}
