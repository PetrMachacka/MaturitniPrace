using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public BuildButton ButtonScript;
    private void OnTriggerEnter(Collider other)
    {
        if (ButtonScript != null)
        {
            ButtonScript.Activate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ButtonScript != null)
        {
            ButtonScript.Activate = false;
        }
    }
}