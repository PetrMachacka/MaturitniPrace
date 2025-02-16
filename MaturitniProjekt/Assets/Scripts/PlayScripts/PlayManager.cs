using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayManager : MonoBehaviour
{
    public GameObject FinishUI;
    public TextMeshProUGUI FinalTime;

    public float timer = 0f;
    private bool isCounting = true;

    void Start()
    {
        StartCoroutine(CountTime());
    }

    IEnumerator CountTime()
    {
        while (isCounting)
        {
            timer += Time.deltaTime;
            FinalTime.text = timer.ToString("F2");
            yield return null;
        }
    }

}
