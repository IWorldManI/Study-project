using UnityEngine;

public class TomatoesStand : Stand
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InventoryManager>())
        {
            Debug.Log("Touch" + name);
            var item = other.GetComponentInChildren<InventoryManager>().ItemRequest(typeof(Tomatoes));
            item.GetComponent<Tomatoes>().Move(transform);
        }
    }
}
