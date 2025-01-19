using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    private Item item;
    public bool WillDeactivate = false;
    [SerializeField] private Transform ButtonTop;
    public bool Activated = false;
    private void Start()
    {
        item = GetComponent<Item>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ToggleButton();
        foreach(Connection i in item.connections)
        {
            if(i.connectedObject != null)
            {
                Item ActiveObject = i.connectedObject.GetComponent<Dot>().ActivateObject.GetComponent<Item>();
                Debug.Log(ActiveObject.name);
                ActiveObject.ActivatedItem = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(WillDeactivate)
        {
            ToggleButton();
            Debug.Log(WillDeactivate);
            foreach (Connection i in item.connections)
            {
                if (i.connectedObject != null)
                {
                    Item ActiveObject = i.connectedObject.GetComponent<Dot>().ActivateObject.GetComponent<Item>();
                    ActiveObject.ActivatedItem = false;
                }
            }
        }
    }
    private void ToggleButton()
    {
        if (!Activated)
        {
            ButtonTop.localPosition -= new Vector3(0, 0.05f, 0);
            Activated = true;
        }
        else if(WillDeactivate)
        {
            ButtonTop.localPosition += new Vector3(0, 0.05f, 0);
            Activated = false;
        }
    }
}
