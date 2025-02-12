using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("jokes");
        if (jsonFile != null)
        {
            Jokes jokes = JsonUtility.FromJson<Jokes>(jsonFile.text);
            if (jokes.death_jokes != null && jokes.death_jokes.Length > 0)
            {
                string randomJoke = jokes.death_jokes[Random.Range(0, jokes.death_jokes.Length)];
                textMeshPro.text = randomJoke;
            }
        }
        else
        {
            Debug.LogError("Jokes file not found in Resources folder");
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("PlayLevel");
    }
}

[System.Serializable]
public class Jokes
{
    public string[] death_jokes;
}