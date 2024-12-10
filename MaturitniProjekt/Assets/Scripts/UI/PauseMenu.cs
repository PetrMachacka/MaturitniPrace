using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject uploadMenu;
    public static bool isPaused = false;
    private void Start() {
        isPaused = false;
        uploadMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    private void OnESCAPE(){
        if(!pauseMenu.activeSelf){
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        else{
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
    public void LevelsButton()
    {
        PlayerPrefs.DeleteKey("SelectedLevel");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Levels");
    }
    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void UploadButton()
    {
        if(!uploadMenu.activeSelf){
            uploadMenu.SetActive(true);
        }
        else{
            uploadMenu.SetActive(false);
        }
    }
    public async void SteamUpload()
    {
        await SteamManager.UploadLevelToSteamWorkshopAsync(PlayerPrefs.GetString("SelectedLevel"));
    }
}
