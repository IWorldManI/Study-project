using System;
using System.Linq;
using UnityEngine;

public class ItemGiver : ItemDistributor
{
    //class for test item pickup from spawner
    protected override void Start()
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

    private void Give(InventoryManager inventoryManager, Type type)
    {
        Debug.Log("Touch " + name);
        if (ItemContains.Count > 0) 
        {
            var item = ItemContains.LastOrDefault(x => x.GetType() == type);
            GiveItem(inventoryManager, item, this);
        }
    } 
   private void OnTriggerEnter(Collider other)
   {
       if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
       {
           var inventoryManager = player.GetComponentInChildren<InventoryManager>();
           {
               if(ItemContains.LastOrDefault(x => x.GetType() == typeof(Tomatoes))) 
                   Give(inventoryManager, typeof(Tomatoes));
               else
                   Give(inventoryManager, typeof(Milk));
           }
       }
   }
}
