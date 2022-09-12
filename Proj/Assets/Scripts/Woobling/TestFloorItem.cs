using UnityEngine;

public class TestFloorItem : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<InventoryManager>())
        {
            //Debug.Log("Touch");
            var inventoryManager = other.GetComponentInChildren<InventoryManager>();
            var item = GetComponent<Ingredient>();
            transform.parent = inventoryManager.transform;
            //_inventoryManager.AddItem(item);
            var test = FindObjectOfType<InventoryManager>();
            test.AddToDictionary(item);
        }
    }
}
