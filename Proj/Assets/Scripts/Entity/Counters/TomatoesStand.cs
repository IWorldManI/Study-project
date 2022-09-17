using UnityEngine;

public class TomatoesStand : Stand
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InventoryManager>())
        {
            Debug.Log("Touch");
            other.GetComponentInChildren<InventoryManager>().ItemRequest(typeof(Tomatoes));
        }
    }
}
