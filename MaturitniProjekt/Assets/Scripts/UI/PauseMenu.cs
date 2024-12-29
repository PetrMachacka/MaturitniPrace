using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject uploadMenu;
    public GameObject canvas;
    public GameObject imageCanvas;
    public TextMeshPro levelNameText;
    public static bool isPaused = false;
    public static bool pictureMode = false;
    private void Start() {
        isPaused = false;
        uploadMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    private void OnESCAPE(){
        if(!pictureMode){
            TogglePauseMenu();
        }
    }
    private void TogglePauseMenu(){
        Debug.Log("TogglePauseMenu");   
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
    private void ToggleUI(){
        Debug.Log("ToggleUI");
        if(!canvas.activeSelf){
            canvas.SetActive(true);
        }
        else{
            canvas.SetActive(false);
        }
    }
    private async void OnPhoto(){
        if(pictureMode)
        {
            TakeScreenshot();
            await Task.Delay(100);
            TogglePauseMenu();
            ToggleUI();
            FillImage();
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
            FillImage();
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
        ToggleUI();
    }
    public void FillImage()
    {
        string selectedLevel = PlayerPrefs.GetString("SelectedLevel");
        string screenshotPath = Path.Combine(Application.persistentDataPath, $"Levels/{selectedLevel}/preview.png");

        if (File.Exists(screenshotPath))
        {
            byte[] fileData = File.ReadAllBytes(screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            RawImage rawImage = imageCanvas.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = texture;
            }
            else
            {
                Debug.LogError("RawImage component not found on imageCanvas.");
            }
        }
        else
        {
            Debug.LogError("Screenshot not found: " + screenshotPath);
        }
    }
}
