using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private GameObject SecondPlayer;
    public void AddPlayer()
    {
        SecondPlayer.SetActive(true);
    }
    

}
