using UnityEngine;

public class MilkStand : Stand
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InventoryManager>())
        {
            Debug.Log("Touch" + name);
            var item = other.GetComponentInChildren<InventoryManager>().ItemRequest(typeof(Milk));
            item.GetComponent<Milk>().Move(transform);
        }
    }
}
