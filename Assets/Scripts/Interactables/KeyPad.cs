using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : Interactables
{
    [SerializeField] GameObject door;
    bool doorOpen;
    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.SetActive(doorOpen);
    }
}
