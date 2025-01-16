using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] public GameObject SensitivitySlider;
    [SerializeField] public GameObject ControllerSensitivitySlider;
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private float controllerSensitivity = 1;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        }
        else
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
            SensitivitySlider.GetComponent<Slider>().value = sensitivity;
        }
        if (!PlayerPrefs.HasKey("ControllerSensitivity"))
        {
            PlayerPrefs.SetFloat("ControllerSensitivity", controllerSensitivity);
        }
        else
        {
            controllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity");
            ControllerSensitivitySlider.GetComponent<Slider>().value = controllerSensitivity;
        }
    }
    public void GetSliderValue()
    {
        sensitivity = SensitivitySlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        Debug.Log(sensitivity);
    }
    public void GetControllerSliderValue()
    {
        controllerSensitivity = ControllerSensitivitySlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("ControllerSensitivity", controllerSensitivity);
        Debug.Log(controllerSensitivity);
    }
}
