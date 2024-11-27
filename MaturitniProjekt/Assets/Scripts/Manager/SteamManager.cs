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

    public static async Task UploadLevelToSteamWorkshopAsync(string SelectedLevel)
    {
        
        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");
        Debug.Log(levelsPath);
        string filePath = levelsPath + "/" + SelectedLevel + ".json";
        Debug.Log(filePath);
        if (filePath == null)
        {
            Debug.LogError("No level file found with the matching GUID: " + SelectedLevel);
            return;
        }

        var result = await Steamworks.Ugc.Editor.NewCommunityFile
					.WithTitle( "My New Item" )
					.WithDescription( "nice" )
					.WithTag( "Map" )
                    .SubmitAsync( new ProgressClass() );
                    
        if (result.Success)
        {
            Debug.Log("Upload successful!");
        }
        else
        {
            Debug.LogError("Upload failed: " + result.Result);
        }
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

