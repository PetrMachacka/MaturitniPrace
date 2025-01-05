using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox : MonoBehaviour
{
    public bool isChecked = false;
    public GameObject checkmark;

    public void Toggle()
    {
        isChecked = !isChecked;
        checkmark.SetActive(isChecked);
    }
}
