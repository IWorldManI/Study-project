using UnityEngine;

public class MilkStand : ItemDistributor
{
    void Start()
    {
        base.Start();
        {
            MaxCapacity = 8;
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            
            MaxCapacity = ItemPlace.Count;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InventoryManager>())
        {
            Debug.Log("Touch" + name);
            var inventoryManager = other.GetComponentInChildren<InventoryManager>();
            var item = other.GetComponentInChildren<InventoryManager>().ItemGiveRequest(typeof(Milk));
            ReceiveItem(inventoryManager,item);
        }
    }
}
