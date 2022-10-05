using Entity.NPC;
using UnityEngine;

public class MilkStand : ItemDistributor
{
    void Start()
    {
        base.Start();
        {
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            
            MaxCapacity = ItemPlace.Count;
            StandType = typeof(Milk);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out var npc) && ItemContains.Count > 0)
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            if(StandType == inventoryManager.LookingItem) //mb use this in base class?
                Give(inventoryManager);
        }
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            Receive(inventoryManager);
        }
    }
    private void Give(InventoryManager inventoryManager)
    {
        var item = ItemContains[ItemContains.Count - 1].GetComponent<Ingredient>();
        base.GiveItem(inventoryManager, item);
        var indx = ItemContains.IndexOf(item);
        ItemContains.RemoveAt(indx);
    }

    private void Receive(InventoryManager inventoryManager)
    {
        var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(typeof(Milk));
        ReceiveItem(inventoryManager, item);
    }
}
