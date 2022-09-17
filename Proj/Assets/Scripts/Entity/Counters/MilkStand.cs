using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkStand : Stand
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InventoryManager>())
        {
            Debug.Log("Touch");
            other.GetComponentInChildren<InventoryManager>().ItemRequest(typeof(Milk));
        }
    }
}
