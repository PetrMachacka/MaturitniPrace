using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using Steamworks.Data;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class LevelPlayButton : MonoBehaviour
{
    public async void DownloadLevel(){
        string id = gameObject.name;
        var item = await SteamManager.GetItemByID(ulong.Parse(id));
        SteamManager.DownloadByID(item.Value);
    }


}

