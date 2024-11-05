using System.Collections;
using System.Collections.Generic;
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
            Debug.LogError("could not initialize steam client: " + e.Message);
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
}