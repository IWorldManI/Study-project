using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloorItem : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<WobblingConnector>())
        {
            //Debug.Log("Touch");
            var _wobbling = other.GetComponentInChildren<WobblingConnector>();
            transform.parent = _wobbling.transform;
            _wobbling.AddItem(transform);
            var test = FindObjectOfType<InventoryManager>();
            test.AddToDictionary(gameObject);
        }
    }
}
