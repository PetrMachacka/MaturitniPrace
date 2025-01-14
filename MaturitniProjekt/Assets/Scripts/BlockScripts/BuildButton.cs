using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public bool Activate;
    public GameObject Trigger;
    private Item item;
    private void Start()
    {
        item = GetComponent<Item>();

    }
    private void Update()
    {
        if (Activate)
        {
            Trigger.SetActive(true);
            foreach(Connection i in item.connections)
            {
                if(i.connectedObject != null)
                {
                    Item ActiveObject = i.connectedObject.GetComponent<Dot>().ActivateObject.GetComponent<Item>();
                    Debug.Log(ActiveObject.Activated);
                    ActiveObject.Activated = true;
                }
            }
        }
    }
}
