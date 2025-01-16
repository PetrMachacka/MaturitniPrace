using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class Item : MonoBehaviour
{
    [Header("Tool Settings")]
    [SerializeField] public bool isTool = false;
    [SerializeField] public bool TwoBlocks = false;
    [SerializeField] public bool Rotating = false;

    [Header("Logic Settings")]
    [SerializeField] public bool isLogic = false;
    [SerializeField] public bool isInput = false;

    [Header("Connections")]
    [SerializeField] public List<Connection> connections = new List<Connection>();
    [Header("Other")]
    [SerializeField] public bool isSpawn = false;
    private GameObject ConnectionDot;
    public bool ActivatedItem;

    private void Start(){
        if(isLogic)
        {
            FindConnectionDot();
        }
    }
    private void Update()
    {
        ToggleConnectionDot();
    }
    private void FindConnectionDot()
    {
        var connectingDotTransform = transform.Find("ConnectingDot");
        if (connectingDotTransform != null)
        {
            ConnectionDot = connectingDotTransform.gameObject;
        }
        else
        {
            var connectedDotTransform = transform.Find("ConnectedDot");
            if (connectedDotTransform != null)
            {
                ConnectionDot = connectedDotTransform.gameObject;
            }
        }
    }
    private void ToggleConnectionDot()
    {
        if(isLogic && ConnectionDot != null && BuildManager.buildMode == BuildModes.logic){
            ConnectionDot.SetActive(true);
        }
        else if(ConnectionDot != null){
            ConnectionDot.SetActive(false);
        }
    }
}