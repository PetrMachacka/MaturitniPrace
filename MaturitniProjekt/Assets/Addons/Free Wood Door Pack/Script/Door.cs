using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour {
	public float smooth = 1.0f;
	float DoorOpenAngle = -90.0f;
    float DoorCloseAngle = 0.0f;
	private Item item;
	[SerializeField] private GameObject Block;
	// Use this for initialization
	void Start () {
		item = Block.GetComponent<Item> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (item.ActivatedItem)
		{
            var target = Quaternion.Euler (0, DoorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
	
		}
		else
		{
            var target1= Quaternion.Euler (0, DoorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
	
		}  
	}
	/*
	public void OpenDoor(){
		open =!open;
		asource.clip = open?openDoor:closeDoor;
		asource.Play ();
	}
	*/
}
