using System;
using System.Collections;
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                DelayRoutine = Delay(inventoryManager);
                StartCoroutine(DelayRoutine);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (DelayRoutine != null) 
            {
                StopCoroutine(DelayRoutine);
            }
        }
    }
    private void Give(InventoryManager inventoryManager, Type type)
    {
        if (ItemContains.Count > 0) 
        {
            var item = ItemContains.LastOrDefault(x => x.GetType() == type);
            GiveItem(inventoryManager, item, ItemContains);
        }
    }

    protected override IEnumerator Delay(InventoryManager inventoryManager)
    {
       yield return new WaitForSeconds(.4f);
       
       Give(inventoryManager, ItemContains.LastOrDefault().GetType());
       
       if (ItemContains.Count > 0)
       {
           DelayRoutine = Delay(inventoryManager);
           StartCoroutine(DelayRoutine);
       }
    }
}
