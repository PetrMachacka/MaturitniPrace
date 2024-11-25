
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
    }
}
