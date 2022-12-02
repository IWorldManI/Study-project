using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.NPC;
using UnityEngine;

public class TrashCan : ItemDistributor
{
    private EventBus _eventBus;
    private readonly Dictionary<string, Coroutine> _giveItemDictionary = new Dictionary<string, Coroutine>(); 
    
    private TomatoesPool _tomatoesPool;
    private MilkPool _milkPool;
    private KetchupPool _ketchupPool;

    private void Awake()
    {
        _eventBus = FindObjectOfType<EventBus>();
    }

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
            
            _tomatoesPool = FindObjectOfType<TomatoesPool>();
            _milkPool = FindObjectOfType<MilkPool>();
            _ketchupPool = FindObjectOfType<KetchupPool>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                if (!_giveItemDictionary.ContainsKey(player.GetInstanceID().ToString()))
                {
                    GiveDelayRoutine = GiveDelay(inventoryManager, null, player.GetInstanceID().ToString());
                    _giveItemDictionary.Add(player.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                    //Debug.Log("Routine started npc" + player.GetInstanceID().ToString() + player. name);
                }
            }
        }
        
        if(other.TryGetComponent<Helper>(out var helper))
        {
            var inventoryManager = helper.GetComponentInChildren<InventoryManager>();

            if (!_giveItemDictionary.ContainsKey(helper.GetInstanceID().ToString()))
            {
                ReceiveDelayRoutine = GiveDelay(inventoryManager, helper, helper.GetInstanceID().ToString());
                _giveItemDictionary.Add(helper.GetInstanceID().ToString(), StartCoroutine(ReceiveDelayRoutine));
                //Debug.Log("Routine started helper" + helper.GetInstanceID().ToString()+ helper.name);   
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_giveItemDictionary.TryGetValue(player.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _giveItemDictionary.Remove(player.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped player" + player.GetInstanceID().ToString());
            }
        }
        
        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (_giveItemDictionary.TryGetValue(helper.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _giveItemDictionary.Remove(helper.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped helper" + helper.GetInstanceID().ToString());
            }
        }
    }
    
    private void DropInCan(InventoryManager inventoryManager)
    {
        var type = inventoryManager._ingredientList.LastOrDefault()?.GetType();
        var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(type);
        ReceiveItem(inventoryManager, item, ItemContains, this);
    }

    protected override void OnCollectAction(ItemDistributor distributor)
    {
        var item = ItemContains.LastOrDefault();
        var index = ItemContains.IndexOf(item);
        if(item.TryGetComponent<Tomatoes>(out var tomatoes))
        {
            _tomatoesPool.KillShape(tomatoes.GetComponent<PooledShape>());
        }
        if (item.TryGetComponent<Milk>(out var milk))
        {
            _milkPool.KillShape(milk.GetComponent<PooledShape>());
        }
        if (item.TryGetComponent<Ketchup>(out var ketchup))
        {
            _ketchupPool.KillShape(ketchup.GetComponent<PooledShape>());
        }
        
        ItemContains.RemoveAt(index);
        Debug.Log(item + " was returned to pool");
    }
    
    private IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc, string entityName)
    {
        while (true)
        {
            if (_giveItemDictionary.TryGetValue(entityName, out Coroutine rCoroutine))
            {
                yield return new WaitForSeconds(ItemDistributeDelay);
                
                Debug.Log("Try give item for " + entityName);
                if (inventoryManager._ingredientList.Count > 0) ;
                    DropInCan(inventoryManager);
            }
            
            yield return null;
        }
    }

    private void OnEnable()
    {
        _eventBus.OnCollectStand += OnCollectAction;
    }

    private void OnDisable()
    {
        _eventBus.OnCollectStand -= OnCollectAction;
    }
}
