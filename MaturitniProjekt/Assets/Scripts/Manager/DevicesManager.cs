using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class DevicesManager : MonoBehaviour
{
    public static List<InputDevice> devices = new List<InputDevice>();

    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Connected device: {device.device}");
            devices.Add(device);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {

        switch (change)
        {
            case InputDeviceChange.Added:
                Debug.Log($"Device connected: {device.displayName}");
                devices.Add(device);
                break;
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device disconnected: {device.displayName}");
                Errors.ShowError("Device disconnected: " + device.displayName);
                devices.Remove(device);
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log($"Device reconnected: {device.displayName}");
                if (!devices.Contains(device))
                {
                    devices.Add(device);
                }
                break;
        }
        PlayerPrefs.SetString("ConnectedDevices", string.Join(",", devices.Select(d => d.displayName).ToArray()));
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("ConnectedDevices"));
    }

    private void OnDestroy()
    {
        // Unsubscribe from device change events
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}