using System.Linq;
using Entity.NPC;
using UnityEngine;

public class ItemGiver : ItemDistributor
{
    void Start()
    {
        base.Start();
        {
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                child.TryGetComponent<Ingredient>(out var item);
                ItemContains.Add(item);
            }
        }
    }

    private void Give(InventoryManager inventoryManager)
    {
        Debug.Log("Touch " + name);
        if (ItemContains.Count > 0) 
        {
            var item = ItemContains[ItemContains.Count - 1].GetComponent<Ingredient>();
            base.GiveItem(inventoryManager, item);
            var indx = ItemContains.IndexOf(item);
            ItemContains.RemoveAt(indx);
        }
    }
    private void OnTriggerEnter(Collider other)
   {
       if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
       {
           var inventoryManager = player.GetComponentInChildren<InventoryManager>();
           {
               Give(inventoryManager);
           }
       }
       if(other.TryGetComponent<NPC>(out var npc))
       {
           var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
           {
               Give(inventoryManager);
           }
       }
   }
}
