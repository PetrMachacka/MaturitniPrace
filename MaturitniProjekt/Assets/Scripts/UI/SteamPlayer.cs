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
        text.text = SteamManager.steamUser;
    }
}
