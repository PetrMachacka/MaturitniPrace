using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;
using Assets.Scripts;
using Unity.VisualScripting;
using TMPro;
public class SteamManager : MonoBehaviour
{
    public static string steamUser;
    public static string steamId;
    private const string AppId = "3336140";
    public static List<String> folderNames = new List<string>();
    public static string workshopPath;
    public static string steamDownloadPath;
    public static bool isSteamActive;
    private void Start()
    {
        workshopPath = GetSteamWorkshopPath();
        ListDownloaded();
        
        try
        {
            SteamUGC.Download(3375074002);
            isSteamActive = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            isSteamActive = false;
        }
    }
    public static void ListDownloaded(){
        if (!string.IsNullOrEmpty(workshopPath))
        {
            steamDownloadPath = Path.Combine(workshopPath, AppId);

            if (Directory.Exists(steamDownloadPath))
            {
            Debug.Log($"Steam Workshop directory for App ID {AppId} found: {steamDownloadPath}");

            string[] directories = Directory.GetDirectories(steamDownloadPath);

            foreach (string directory in directories)
            {
                folderNames.Add(Path.GetFileName(directory));
            }
            }
            else
            {
            Debug.LogWarning($"Steam Workshop directory for App ID {AppId} not found.");
            }
        }
        else
        {
            Debug.LogError("Steam installation directory could not be located.");
        }
    }
    private void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(3336140);
            steamUser = Steamworks.SteamClient.Name.ToString();
            steamId = Steamworks.SteamClient.SteamId.ToString();
            Debug.Log(steamUser);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        Steamworks.SteamClient.Shutdown();
    }

    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    public static async Task UploadLevelToSteamWorkshopAsync(string selectedLevel)
    {
        string directoryPath = FileHelpers.GetFolderPathByGuid(selectedLevel);
        Debug.Log(directoryPath);
        if (directoryPath == null)
        {
            Debug.LogError("No level file found with the matching GUID: " + selectedLevel);
            return;
        }

        string levelDataPath = Path.Combine(directoryPath, "levelData.json");
        LevelData levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(levelDataPath));

        Steamworks.Ugc.Editor editor;
        if (!string.IsNullOrEmpty(levelData.steamId))
        {
            Debug.Log("Level already uploaded to Steam Workshop");
            editor = new Steamworks.Ugc.Editor(ulong.Parse(levelData.steamId));
        }
        else
        {
            editor = Steamworks.Ugc.Editor.NewCommunityFile;
        }

        var result = await editor
            .WithTitle(levelData.name)
            .WithDescription("Description")
            .WithTag(levelData.isCoop ? "Coop" : "Singleplayer")
            .WithTag("Map")
            .WithPreviewFile(Path.Combine(directoryPath, "preview.png"))
            .WithContent(directoryPath)
            .WithPublicVisibility()
            .SubmitAsync(new ProgressClass());

        Debug.Log(result.FileId);
        if (result.Success)
        {
            Debug.Log("Upload successful!");
            levelData.steamId = result.FileId.ToString();
            File.WriteAllText(levelDataPath, JsonUtility.ToJson(levelData, true));
        }
        else
        {
            Debug.LogError("Upload failed: " + result.Result);
        }
    }

    public static async Task<Steamworks.Ugc.ResultPage?> GetLevelListWorkshop(int page = 1, string textSearch = null)
    {   
        
        WorkshopSearchOptions searchOptions = Filters.searchOption;
        Debug.Log(Filters.searchOption.ToString());
        var query = Steamworks.Ugc.Query.Items
            .MatchAnyTag();
        if(Filters.isCoop)
        {
            query = query.WithTag("Coop");
            Debug.Log("Coop");
        }

        if(searchOptions == WorkshopSearchOptions.SortByDate)
        {
            query = query.RankedByPublicationDate();
        }
        else if(searchOptions == WorkshopSearchOptions.sortByVote)
        {
            query = query.SortByVoteScore();
        }
        else if(searchOptions == WorkshopSearchOptions.madeByFriends)
        {
            query = query.CreatedByFriends();
        }
        else if(searchOptions == WorkshopSearchOptions.mostPlayed)
        {
            query = query.RankedByTotalPlaytime();
        }
        else if(searchOptions == WorkshopSearchOptions.trending)
        {
            query = query.RankedByTrend();
        }

        var result = await query.GetPageAsync(page);
        foreach (var item in result.Value.Entries)
        {
            Debug.Log(item.PreviewImageUrl);
        }
        Debug.Log(result.Value.TotalCount);
        return  result;
    }
    public static async Task DownloadByID(Steamworks.Ugc.Item steamItem)
    {
        await steamItem.Subscribe();
        Debug.Log(steamItem.Id);
        SteamUGC.Download(steamItem.Id);
    }
    public static async Task<Steamworks.Ugc.Item?> GetItemByID(ulong id)
    {
        var result = await Steamworks.Ugc.Item.GetAsync(id);
        return result;
    }
    public static async void DeleteItem(Steamworks.Ugc.Item? item)
    {
        await Steamworks.SteamUGC.DeleteFileAsync(item.Value.Id);
    }

    class ProgressClass : IProgress<float>
    {
        float lastvalue = 0;

        public void Report( float value )
        {
            if ( lastvalue >= value ) return;

            lastvalue = value;

            Debug.Log( value );
        }
    }
    private string GetSteamWorkshopPath()
    {
        string steamPath = "";

        // Default paths for different operating systems
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            steamPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            steamPath = Path.Combine(steamPath, "Steam", "steamapps", "workshop", "content");
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            steamPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", "Steam", "steamapps", "workshop", "content");
        }
        else if (Application.platform == RuntimePlatform.LinuxPlayer)
        {
            steamPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".steam", "steam", "steamapps", "workshop", "content");
        }

        if (Directory.Exists(steamPath))
        {
            return steamPath;
        }

        return null;
    }

}

