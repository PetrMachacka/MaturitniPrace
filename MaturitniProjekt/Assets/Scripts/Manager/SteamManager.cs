using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
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

    public static async Task UploadLevelToSteamWorkshopAsync(string levelGuid)
    {
        Debug.Log("Uploading...");

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");
        string filePath = GetLevelFilePath(levelsPath, levelGuid);
        Debug.Log(filePath);
        if (filePath == null)
        {
            Debug.LogError("No level file found with the matching GUID: " + levelGuid);
            return;
        }

        var result = await Steamworks.Ugc.Editor.NewCommunityFile
            .WithTitle("Level")
            .WithDescription("Level")
            .WithTag("Level")
            .WithContent(filePath)
            .SubmitAsync( new ProgressClass() );

        Debug.Log(result.Success);

        if (result.Success)
        {
            Debug.Log("Upload successful!");
        }
        else
        {
            Debug.LogError("Upload failed: " + result.Result);
        }
    }

    private static string GetLevelFilePath(string levelsPath, string levelGuid)
    {
        if (!Directory.Exists(levelsPath))
        {
            Debug.LogError("Levels directory not found: " + levelsPath);
            return null;
        }

        string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");

        foreach (string levelFile in levelFiles)
        {
            string json = File.ReadAllText(levelFile);
            Debug.Log(json);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            if (levelData.guid == levelGuid)
            {
                return levelFile;
            }
        }

        return null;
    }
    class ProgressClass : IProgress<float>
    {
        float lastvalue = 0;

        public void Report( float value )
        {
            if ( lastvalue >= value ) return;

            lastvalue = value;

            Console.WriteLine( value );
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public string guid;
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }
}

