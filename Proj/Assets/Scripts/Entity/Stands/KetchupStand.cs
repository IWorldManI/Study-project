using Entity.NPC;
using UnityEngine;

public class KetchupStand : ItemDistributor
{
    protected override void Start()
    {
        base.Start();
        {
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            
            MaxCapacity = ItemPlace.Count;
            StandType = typeof(Ketchup);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out var npc) && ItemContains.Count > 0)
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            var item = ItemContains[^1].GetComponent<Ingredient>();
            if(StandType == inventoryManager.LookingItem) //mb use this in base class?
                GiveItem(inventoryManager,item,this);
            StartCoroutine(npc.NextState());
        }
        else if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(typeof(Ketchup));
            ReceiveItem(inventoryManager, item);
        }
    }
}
