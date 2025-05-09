using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public List<GameObject> Tutorials;
    public List<GameObject> TutorialButtons;
    void Start()
    {
        OpenTutorial(0);   
    }
    public void OpenTutorial(int index)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (i == index)
            {
                Tutorials[i].SetActive(true);
                TutorialButtons[i].SetActive(false);
            }
            else
            {
                Tutorials[i].SetActive(false);
                TutorialButtons[i].SetActive(true);
            }
        }
    }
}
