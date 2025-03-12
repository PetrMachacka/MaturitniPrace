using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    Camera camera1;
    Camera camera2;
    GameObject finishMenu;
    private void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        LoadLevel loadLevelScript = manager.GetComponent<LoadLevel>();
        if(SceneManager.GetActiveScene().name == "PlayLevel")
        {
            finishMenu = loadLevelScript.GetComponent<PlayManager>().FinishUI;
        }
        if(LoadLevel.isCoop)
        {
            camera1 = loadLevelScript.CharacterA.transform.Find("Camera").GetComponent<Camera>();
            camera2 = loadLevelScript.CharacterB.transform.Find("Camera").GetComponent<Camera>();
        }
    }
    bool Player1Reached = false;
    bool Player2Reached = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if(!LoadLevel.isCoop){
                FinishLevel();
            }
            else
            {
                Debug.Log("Player1" + Player1Reached);
                Debug.Log("Player2" + Player2Reached);
                if(other.gameObject.name == "CharacterA")
                {
                    if(!Player2Reached){
                        camera2.rect = new Rect(0, 0, 1, 1);
                        Destroy(other.gameObject);
                    }
                    Player1Reached = true;
                    Debug.Log("Character A has reached the end");
                }
                else if(other.gameObject.name == "CharacterB")
                {
                    if(!Player1Reached) {
                        camera1.rect = new Rect(0, 0, 1, 1); 
                        Destroy(other.gameObject);
                    }
                    Player2Reached = true;
                    Debug.Log("Character B has reached the end");
                }
            }
            if(Player1Reached && Player2Reached)
            {
                FinishLevel();
            }
        }
    }
    private void FinishLevel()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        finishMenu.SetActive(true);
    }
}
