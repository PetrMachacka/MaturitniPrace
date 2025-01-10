using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonScript : MonoBehaviour
{
    public bool isCoop;
    public void SelectLevel()
    {
        string levelGuid = gameObject.name;
        Debug.Log("Selected Level GUID: " + levelGuid);
        PlayerPrefs.SetString("SelectedLevel", levelGuid);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("EditorScene");
    }
    public void DeleteLevel()
    {
        string levelGuid = gameObject.name;
        Debug.Log("Deleting Level GUID: " + levelGuid);

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");
        string directoryPath = Path.Combine(levelsPath, levelGuid);
        if (Directory.Exists(directoryPath))
        {
            Debug.Log("Level file found: " + directoryPath);
            Directory.Delete(directoryPath, true);
        }
        else
        {
            Debug.LogError("Level file not found: " + directoryPath);
        }

        SceneManager.LoadScene("Levels");

    }
    public void PlayLevel()
    {
        Debug.Log(PlayerPrefs.GetInt("Coop"));
        Debug.Log(isCoop);
        if(isCoop && PlayerPrefs.GetInt("Coop") == 0)
        {
            Errors.ShowError("You need 2 Players for COOP.");
            return;
        }
        else if(!isCoop && PlayerPrefs.GetInt("Coop") == 1)
        {
            Errors.ShowError("You need 1 Player for Single Player.");
            return;
        }
        string levelGuid = gameObject.name;
        Debug.Log("Playing Level GUID: " + levelGuid);
        PlayerPrefs.SetString("SelectedLevel", levelGuid);
        PlayerPrefs.Save();

        SceneManager.LoadScene("PlayLevel");
    }
}
