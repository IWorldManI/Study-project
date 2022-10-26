using System.Collections;
using System.Collections.Generic;
using Entity.NPC;
using UnityEngine;

public class TomatoesStand : ItemDistributor
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
            StandType = typeof(Tomatoes);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out var npc))
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, npc);
                StartCoroutine(GiveDelayRoutine);
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
    
    private IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc)
    {
        yield return new WaitForSeconds(_itemDistributeDelay);
        
        GiveDelayRoutine = GiveDelay(inventoryManager, npc);
        StartCoroutine(GiveDelayRoutine);
        
        if (StandType == inventoryManager.LookingItem && ItemContains.Count > 0)
        {
            var item = ItemContains[^1].GetComponent<Ingredient>();
            GiveItem(inventoryManager, item, ItemContains);
            npc.OnCollect += npc.OrderNext;
            npc.OnCollect?.Invoke();
            npc.OnCollect -= npc.OrderNext;
            StopCoroutine(GiveDelayRoutine);
        }
    }
}
