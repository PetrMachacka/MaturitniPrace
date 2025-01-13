using Unity.VisualScripting;
using UnityEngine;


public class LevelPlayButton : MonoBehaviour
{
    private Steamworks.Ugc.Item? item;
    private async void Start()
    {
        string id = gameObject.name;
        Debug.Log(id);
        item = await SteamManager.GetItemByID(ulong.Parse(id));
    }
    public void DownloadLevel(){
        SteamManager.DownloadByID(item.Value);
    }
    public void PlayLevel(){
        string id = gameObject.name;
        PlayerPrefs.SetString("SelectedLevel", id);
        PlayerPrefs.SetInt("NewLevelInt", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelPlay");
    }
    public void DeleteLevel(){
        Debug.Log(item.Value.Title);
        if(SteamManager.steamId == item.Value.Owner.Id.ToString()){
            SteamManager.DeleteItem(item.Value.Id.ToString());
        }
    }
    public void GetPreviewPicture(){
        var preview = item.Value.PreviewImageUrl;
        Debug.Log(item.Value.Title);
        Debug.Log(preview);
    }
}

