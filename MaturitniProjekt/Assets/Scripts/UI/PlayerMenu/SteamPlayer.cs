using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SteamPlayer : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Start()
    {
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (SteamManager.steamUser != null)
        {
            text.text = SteamManager.steamUser;
        }
        else
        {
            text.text = "Player1";
        }
    }
}
