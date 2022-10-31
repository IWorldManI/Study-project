using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.NPC;
using UnityEngine;

public class MilkStand : ItemDistributor
{
    private readonly Dictionary<NPC, Coroutine> _receiveItemDictionary = new Dictionary<NPC, Coroutine>();
    private readonly Dictionary<NPC, Coroutine> _giveItemDictionary = new Dictionary<NPC, Coroutine>(); //maybe helper class
    
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
        if(other.TryGetComponent<NPC>(out var npc))
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            if (!_receiveItemDictionary.ContainsKey(npc))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, npc);
                _receiveItemDictionary.Add(npc, StartCoroutine(GiveDelayRoutine));
                Debug.Log("Routine started npc" + npc.name);
            }
        }
        else if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                ReceiveDelayRoutine = ReceiveDelay(inventoryManager);
                StartCoroutine(ReceiveDelayRoutine);
                Debug.Log("Routine started player");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (ReceiveDelayRoutine != null) 
            {
                StopCoroutine(ReceiveDelayRoutine);
                Debug.Log("Routine stopped player");
            }
        }

        if (other.TryGetComponent<NPC>(out var npc))
        {
            if (_receiveItemDictionary.TryGetValue(npc, out Coroutine rCoroutine))
            {
                _receiveItemDictionary.Remove(npc);

                StopCoroutine(rCoroutine);
                Debug.Log("Routine stopped npc" + npc.name);
            }
        }
    }

    protected IEnumerator ReceiveDelay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(_itemDistributeDelay);

        if (ItemContains.Count < MaxCapacity) 
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(StandType);
            ReceiveItem(inventoryManager, item, ItemContains);
        }
       
        ReceiveDelayRoutine = ReceiveDelay(inventoryManager);
        StartCoroutine(ReceiveDelayRoutine);
    }
    
    protected IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc)
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
