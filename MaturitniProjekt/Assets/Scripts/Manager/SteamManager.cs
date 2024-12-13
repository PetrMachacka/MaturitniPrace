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
    private void Start()
    {
        SteamUGC.Download(3375074002);
    }
    private void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(3336140);
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

    public static async Task UploadLevelToSteamWorkshopAsync(string SelectedLevel)
    {
        string directoryPath = FileHelpers.GetFolderPathByGuid(SelectedLevel);
        Debug.Log(directoryPath);
        if (directoryPath == null)
        {
            Debug.LogError("No level file found with the matching GUID: " + SelectedLevel);
            return;
        }
        string levelDataPath = Path.Combine(directoryPath, "levelData.json");
        LevelData levelData =  JsonUtility.FromJson<LevelData>(File.ReadAllText(directoryPath + "/levelData.json"));
        if(levelData.steamId != null)
        {
            Debug.LogError("Level already uploaded to Steam Workshop");
            var result = await new Steamworks.Ugc.Editor( levelData.steamId )
					.WithTitle( levelData.name )
					.WithDescription( "Description" )
					.WithTag( "Map" )
                    .WithPreviewFile( directoryPath + "/preview.png" )
                    .WithContent( directoryPath )
                    .WithPublicVisibility()
                    .SubmitAsync( new ProgressClass() );
            return;
        }
        Debug.Log(levelData.name);
        var result = await Steamworks.Ugc.Editor.NewCommunityFile
					.WithTitle( levelData.name )
					.WithDescription( "Description" )
					.WithTag( "Map" )
                    .WithPreviewFile( directoryPath + "/preview.png" )
                    .WithContent( directoryPath )
                    .WithPublicVisibility()
                    .SubmitAsync( new ProgressClass() );
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

    public static async Task<Steamworks.Ugc.ResultPage?> GetLevelListWorkshop(WorkshopSearchOptions searchOptions, int page = 1, string textSearch = null)
    {
        var query = Steamworks.Ugc.Query.Items
            .WithTag("Map")
            .MatchAnyTag();
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
        
        return  result;
    }
    public static async void DownloadByID( Steamworks.Ugc.Item steamItem)
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
}

