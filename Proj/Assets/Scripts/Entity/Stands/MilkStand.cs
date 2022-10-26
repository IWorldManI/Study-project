using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.NPC;
using UnityEngine;

public class MilkStand : ItemDistributor
{
    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();

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
            var item = ItemContains[^1].GetComponent<Ingredient>();
            if (StandType == inventoryManager.LookingItem)
            {
                GiveItem(inventoryManager, item, ItemContains);
                npc.OnCollect += npc.OrderNext;
                npc.OnCollect?.Invoke();
                npc.OnCollect -= npc.OrderNext;
            }
        }
        else if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                DelayRoutine = ReceiveDelay(inventoryManager);
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

    private IEnumerator ReceiveDelay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(_itemDistributeDelay);

        if (ItemContains.Count < MaxCapacity) 
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(StandType);
            ReceiveItem(inventoryManager, item, ItemContains);
        }
       
        DelayRoutine = ReceiveDelay(inventoryManager);
        StartCoroutine(DelayRoutine);
    }
}
