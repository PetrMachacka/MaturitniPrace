using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonScript : MonoBehaviour
{
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
        string filePath = Path.Combine(levelsPath, levelGuid + ".json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Level file deleted: " + filePath);
        }
        else
        {
            Debug.LogError("Level file not found: " + filePath);
        }

        SceneManager.LoadScene("Levels");

    }
}
