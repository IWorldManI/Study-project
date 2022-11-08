using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.NPC;
using UnityEngine;

public class ItemGiver : ItemDistributor
{
    private readonly Dictionary<string, Coroutine> _receiveItemDictionary = new Dictionary<string, Coroutine>();
    private readonly Dictionary<string, Coroutine> _giveItemDictionary = new Dictionary<string, Coroutine>(); 
    
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
                if (!_receiveItemDictionary.ContainsKey(player.GetInstanceID().ToString()))
                {
                    GiveDelayRoutine = GiveDelay(inventoryManager, null, player.GetInstanceID().ToString());
                    _receiveItemDictionary.Add(player.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                    //Debug.Log("Routine started npc" + player.GetInstanceID().ToString() + player. name);
                }
            }
        }
        if (other.TryGetComponent<Helper>(out var helper))
        {
            var inventoryManager = helper.GetComponentInChildren<InventoryManager>();
            if (!_receiveItemDictionary.ContainsKey(helper.GetInstanceID().ToString()))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, helper, helper.GetInstanceID().ToString());
                _receiveItemDictionary.Add(helper.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                //Debug.Log("Routine started npc" + helper.GetInstanceID().ToString()+ helper.name);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_receiveItemDictionary.TryGetValue(player.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _receiveItemDictionary.Remove(player.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped helper" + player.GetInstanceID().ToString() + player.name);
            }
        }
        
        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (_receiveItemDictionary.TryGetValue(helper.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _receiveItemDictionary.Remove(helper.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped helper" + helper.GetInstanceID().ToString()+ helper.name);
            }
        }
    }

   
    private IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc, string entityName) //override in stand WARNING
    {
        while (true)
        {
            yield return new WaitForSeconds(ItemDistributeDelay);
            
            if (!_receiveItemDictionary.ContainsKey(entityName))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, npc, entityName);
                _receiveItemDictionary.Add(entityName, StartCoroutine(GiveDelayRoutine));
            }
        
            TryGiveItem(inventoryManager, npc);
            yield return null;
        }
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
