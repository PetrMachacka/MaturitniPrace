using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [HideInInspector] public Guid id;
    private void Start()
    {
        id = Guid.NewGuid();
    }
}
