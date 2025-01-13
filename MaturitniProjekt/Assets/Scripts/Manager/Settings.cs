using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] public GameObject SensitivitySlider;
    [SerializeField] private float sensitivity = 300;
    public void GetSliderValue()
    {
        sensitivity = SensitivitySlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        Debug.Log(sensitivity);
    }
}
