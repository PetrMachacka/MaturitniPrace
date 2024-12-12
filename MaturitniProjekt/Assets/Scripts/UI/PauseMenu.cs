using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject uploadMenu;
    public static bool isPaused = false;
    public static bool pictureMode = false;
    private void Start() {
        isPaused = false;
        uploadMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    private void OnESCAPE(){
        TogglePauseMenu();
    }
    private void TogglePauseMenu(){
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
    private void OnPhoto(){
        Debug.Log("nice");
        if(pictureMode)
        {
            TakeScreenshot();
            pictureMode = false;
        }
    }
    private void TakeScreenshot()
    {
        string screenshotFolder = Path.Combine(Application.persistentDataPath, $"Levels/{PlayerPrefs.GetString("SelectedLevel")}");
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }

        string screenshotName = $"preview.png";
        string screenshotPath = Path.Combine(screenshotFolder, screenshotName);

        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log($"Screenshot saved to: {screenshotPath}");
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
    public void PictureButton()
    {
        pictureMode = true;
        TogglePauseMenu();
    }
}
