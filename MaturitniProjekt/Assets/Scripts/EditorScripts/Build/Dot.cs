using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Guid id;
    public GameObject ActivateObject;
    private void Start()
    {
        id = Guid.NewGuid();
        Debug.Log(id);
    }
}
