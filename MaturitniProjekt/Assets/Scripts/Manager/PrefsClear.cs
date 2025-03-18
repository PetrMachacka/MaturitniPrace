using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsClear : MonoBehaviour
{


    private static bool hasClearedPrefs = false; // Static flag to track if it has been cleared

    void Awake()
    {
        if (!hasClearedPrefs) // Only clear PlayerPrefs once
        {
            ClearSpecificPrefs(); // Call function to clear specific prefs
            hasClearedPrefs = true; // Mark as cleared
        }

        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    void ClearSpecificPrefs()
    {
        PlayerPrefs.DeleteKey("OpenMenu");

        PlayerPrefs.Save();
    }
}

