using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject LevelEditSelection;
    public GameObject LevelPlaySelection;
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
}
