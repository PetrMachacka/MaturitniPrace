using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LevelPlayButton : MonoBehaviour
{
    public GameObject DeleteButton;
    public GameObject DownloadButton;
    private Steamworks.Ugc.Item? item;
    public string ImageURL;
    [SerializeField] private bool isDownloaded = false;
    private async void Start()
    {
        string id = gameObject.name;
        item = await SteamManager.GetItemByID(ulong.Parse(id));
        if(item.Value.Owner.Id.ToString() == SteamManager.steamId.ToString() && !isDownloaded){
            float offset = DownloadButton.GetComponent<RectTransform>().sizeDelta.x / 2;
            DownloadButton.GetComponent<RectTransform>().sizeDelta = new Vector2(offset, DownloadButton.GetComponent<RectTransform>().sizeDelta.y);
            DownloadButton.GetComponent<RectTransform>().localPosition = new Vector3(DownloadButton.GetComponent<RectTransform>().localPosition.x + (offset / 2), DownloadButton.GetComponent<RectTransform>().localPosition.y, DownloadButton.GetComponent<RectTransform>().localPosition.z);
            DeleteButton.SetActive(true);
        }
    }
    public async void DownloadLevel()
    {
        if (item.HasValue)
        {
            await SteamManager.DownloadByID(item.Value);
        }
        SteamManager.folderNames.Add(gameObject.name);
        await Refresh();
    }
    public void PlayLevel(){
        string id = gameObject.name;
        bool mode = item.Value.HasTag("Coop") ? true : false;
        if(mode && PlayerPrefs.GetInt("Coop") == 0)
        {
            Errors.ShowError("You need 2 Players for COOP.");
            return;
        }
        else if(!mode && PlayerPrefs.GetInt("Coop") == 1)
        {
            Errors.ShowError("You need 1 Player for Single Player.");
            return;
        }
        if(isDownloaded){
            PlayerPrefs.SetInt("SteamDownloads", 1);
            PlayerPrefs.SetString("SelectedLevel", id);
            SceneManager.LoadScene("PlayLevel");
        }
    }
    public async void UnPublish(){
        Debug.Log(item.Value.Title);
        if(SteamManager.steamId == item.Value.Owner.Id.ToString()){
            Debug.Log("Unpublishing " + item.Value.Title);
            SteamManager.DeleteItem(item);
        }
        await Refresh();
    }
    public async void DeleteLevel(){
        string path = SteamManager.steamDownloadPath + "/" + gameObject.name;
        SteamManager.folderNames.Remove(gameObject.name);
        if(Directory.Exists(path)){
            Directory.Delete(path, true);
        }
        await Refresh();
    }
    public async void GetPreviewPicture()
    {
        if (!string.IsNullOrEmpty(ImageURL))
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(ImageURL))
            {
                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    GameObject previewImageObject = GameObject.Find("PreviewImage");
                    if (previewImageObject != null)
                    {
                        Image previewImage = previewImageObject.GetComponent<Image>();
                        if (previewImage != null)
                        {
                            previewImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("ImageURL is empty or null.");
        }
    }
    private async Task Refresh(){
        await GameObject.Find("LevelSelection").GetComponent<LevelPlayMenu>().LoadWorkshopLevels(PlayerPrefs.GetInt("Page", 1));

    }
}

