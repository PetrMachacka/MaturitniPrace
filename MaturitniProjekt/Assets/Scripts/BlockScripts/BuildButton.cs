using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public bool Activate;
    private Item item;
    public bool deactivateAfter = false;
    private void Start()
    {
        item = GetComponent<Item>();
    }
    private void Update()
    {
        if (Activate)
        {
            Debug.Log(item.connections.Count);
            foreach(Connection i in item.connections)
            {
                if(i.connectedObject != null)
                {
                    Item ActiveObject = i.connectedObject.GetComponent<Dot>().ActivateObject.GetComponent<Item>();
                    Debug.Log(ActiveObject.name);
                    ActiveObject.Activated = true;
                }
            }
        }
        else if(deactivateAfter)
        {
            foreach (Connection i in item.connections)
            {
                if (i.connectedObject != null)
                {
                    Item ActiveObject = i.connectedObject.GetComponent<Dot>().ActivateObject.GetComponent<Item>();
                    ActiveObject.Activated = false;
                }
            }
        }
    }
}
