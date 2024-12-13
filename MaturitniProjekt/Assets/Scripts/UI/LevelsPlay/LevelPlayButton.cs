using UnityEngine;


public class LevelPlayButton : MonoBehaviour
{
    public async void DownloadLevel(){
        string id = gameObject.name;
        var item = await SteamManager.GetItemByID(ulong.Parse(id));
        SteamManager.DownloadByID(item.Value);
    }
    public void PlayLevel(){
        string id = gameObject.name;
        PlayerPrefs.SetString("SelectedLevel", id);
        PlayerPrefs.SetInt("NewLevelInt", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelPlay");
    }
    
}

