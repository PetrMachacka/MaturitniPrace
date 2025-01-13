using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using Assets.Scripts;
using System.Linq.Expressions;
public class DevicesManager : MonoBehaviour
{
    public static List<DeviceTypes> devices = new List<DeviceTypes>();
    private static bool hasDeletedPlayerPrefs = false;
    private bool openMenuCleared = false;
    void Start()
    {
        if(!openMenuCleared)
        {
            PlayerPrefs.SetString("OpenMenu", "");
            PlayerPrefs.Save();
            openMenuCleared = true;
        }
        InputSystem.onDeviceChange += OnDeviceChange;
        if (!hasDeletedPlayerPrefs)
        {
            PlayerPrefs.DeleteKey("ConnectedDevices");
            PlayerPrefs.DeleteKey("Coop");
            PlayerPrefs.Save();
            hasDeletedPlayerPrefs = true;
        }
        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Connected device: {device.device}");
            devices.Add(ConvertToDeviceType(device));
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {

        switch (change)
        {
            case InputDeviceChange.Added:
                Debug.Log($"Device connected: {device.displayName}");
                devices.Add(ConvertToDeviceType(device));
                break;
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device disconnected: {device.displayName}");
                Errors.ShowError("Device disconnected: " + device.displayName);
                if(ConvertToDeviceType(device) == DeviceTypes.Controller)
                {
                    PlayerPrefs.SetInt("Coop", 0);
                }
                devices.Remove(ConvertToDeviceType(device));
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log($"Device reconnected: {device.displayName}");
                Errors.ShowError("Device reconnected: " + device.displayName);
                if (!devices.Contains(ConvertToDeviceType(device)))
                {
                    devices.Add(ConvertToDeviceType(device));
                }
                break;
        }
        PlayerPrefs.SetString("ConnectedDevices", string.Join(",", devices.Select(d => d).ToArray()));
        PlayerPrefs.Save();
    }
    /*private bool AvailableDevice(DeviceTypes deviceType)
    {
        return devices.Any(d => d.DeviceTypes == deviceType);
    }*/
    private DeviceTypes ConvertToDeviceType(InputDevice input)
    {
        switch (input.displayName.ToLower())
        {
            case "keyboard":
                return DeviceTypes.Keyboard;
            case "mouse":
                return DeviceTypes.Mouse;
            default:
                if (input.displayName.ToLower().Contains("controller"))
                {
                    return DeviceTypes.Controller;
                }
                throw new ArgumentException($"Unknown device type: {input.displayName}");
        }
    }
}