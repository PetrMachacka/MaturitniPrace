using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevicesManager : MonoBehaviour
{
    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Connected device: {device.displayName}");
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                Debug.Log($"Device connected: {device.displayName}");
                break;
            case InputDeviceChange.Removed:
                Debug.Log($"Device disconnected: {device.displayName}");
                break;
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device disconnected: {device.displayName}");
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log($"Device reconnected: {device.displayName}");
                break;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from device change events
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}