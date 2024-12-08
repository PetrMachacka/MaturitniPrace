using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject LevelEditSelection;
    public GameObject LevelPlaySelection;
    public GameObject LevelPlayCommunitySelection;

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlaySelection()
    {
        if(LevelPlaySelection.activeSelf)
        {
            LevelPlaySelection.SetActive(false);
        }
        else
        {
            LevelPlaySelection.SetActive(true);
        }
    }
    public void EditSelection()
    {
        if(LevelEditSelection.activeSelf)
        {
            LevelEditSelection.SetActive(false);
        }
        else
        {
            LevelEditSelection.SetActive(true);
        }
    }
    public void PlayCommunitySelection()
    {
        if(LevelPlayCommunitySelection.activeSelf)
        {
            LevelPlayCommunitySelection.SetActive(false);
        }
        else
        {
            LevelPlayCommunitySelection.SetActive(true);
        }
    }
}
