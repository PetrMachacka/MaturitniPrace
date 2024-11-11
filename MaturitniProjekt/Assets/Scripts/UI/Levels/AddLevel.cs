using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class AddLevel : MonoBehaviour
{
    public GameObject AddLevelUI;
    public TMP_InputField inputField;
    private string levelName;

    void Start()
    {
        AddLevelUI.SetActive(false);
    }

    public void OpenAddLevelUI()
    {
        AddLevelUI.SetActive(true);
    }
    public void CloseAddLevelUI()
    {
        AddLevelUI.SetActive(false);
    }
    public void ReadInputFieldText()
    {
        levelName = inputField.text;
        Debug.Log(inputField.text);
    }
    public void CreateNewLevel()
    {
        if(levelName == null)
        {
            Debug.Log("Level name is null");
            return;
        }
        PlayerPrefs.SetString("NewLevel", levelName);
        PlayerPrefs.SetInt("NewLevelInt", 1);

        PlayerPrefs.Save();
        
        SceneManager.LoadScene("EditorScene");
    }
}
