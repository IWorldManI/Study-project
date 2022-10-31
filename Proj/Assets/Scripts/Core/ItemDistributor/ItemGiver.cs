using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGiver : ItemDistributor
{
    private TomatoesPool _tomatoesPool;
    
    private MilkPool _milkPool;
    
    //class for test item pickup from spawner
    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();
            
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                child.TryGetComponent<Ingredient>(out var item);
                ItemContains.Add(item);
            }
            MaxCapacity = ItemContains.Count;
            _tomatoesPool = GetComponentInChildren<TomatoesPool>();
            _milkPool = GetComponentInChildren<MilkPool>();
            StartCoroutine(SpawnDelay());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                ReceiveDelayRoutine = Delay(inventoryManager);
                StartCoroutine(ReceiveDelayRoutine);
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

    private IEnumerator Delay(InventoryManager inventoryManager)
    {
       yield return new WaitForSeconds(_itemDistributeDelay);
       if (ItemContains.Count > 0) 
           Give(inventoryManager, ItemContains.LastOrDefault().GetType());
       
       ReceiveDelayRoutine = Delay(inventoryManager);
       StartCoroutine(ReceiveDelayRoutine);
    }
    private IEnumerator SpawnDelay()
    {
        while (true)
        {
            if (ItemContains.Count < MaxCapacity)
            {
                var item = _tomatoesPool.Spawn(transform.position + new Vector3(2,0,2));
                ItemContains.Add(item.GetComponent<Ingredient>());
                item = _milkPool.Spawn(transform.position + new Vector3(3,0,2));
                ItemContains.Add(item.GetComponent<Ingredient>());
            }
            yield return new WaitForSeconds(4f);
        }
    }
}
