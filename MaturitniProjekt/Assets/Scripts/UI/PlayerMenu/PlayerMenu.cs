using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private GameObject SecondPlayer;
    private EditorInputSystem editorInputSystem;
    private void Start()
    {
        editorInputSystem = new EditorInputSystem();
        if(PlayerPrefs.GetInt("Coop") == 1)
        {
            SecondPlayer.SetActive(true);
        }
    }
    private void Update()
    {
        if(PlayerPrefs.GetInt("Coop") == 0 && SecondPlayer.activeSelf)
        {
            SecondPlayer.SetActive(false);
        }
    }
    private void OnAddPlayer()
    {
        SecondPlayer.SetActive(true);
        PlayerPrefs.SetInt("Coop", 1);
        Debug.Log("Player Added");
    }
    public void OnRemovePlayer()
    {
        SecondPlayer.SetActive(false);
        PlayerPrefs.SetInt("Coop", 0);
        Debug.Log("Player Removed");
    }
}
