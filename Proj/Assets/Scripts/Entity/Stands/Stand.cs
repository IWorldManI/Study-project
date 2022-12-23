using System.Collections;
using System.Collections.Generic;
using Entity.NPC;
using ModestTree;
using UnityEngine;

public class Stand : AbstractStand, IEnumTypes
{
    [SerializeField] private IEnumTypes.ItemTypes  selectedTypeOfStand;
    
    private void Awake()
    {
        _eventBus = FindObjectOfType<EventBus>();
    }

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
            StandType = IEnumTypes.DictionaryOfStandTypes[selectedTypeOfStand];
        }
    }
    protected override void OnCollectAction(ItemDistributor distributor)
    {
        Debug.Log(name + " was collect item");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            
            if (!_receiveItemDictionary.ContainsKey(player.GetInstanceID().ToString()))
            {
                ReceiveDelayRoutine = ReceiveDelay(inventoryManager, null, player.GetInstanceID().ToString());
                _receiveItemDictionary.Add(player.GetInstanceID().ToString(), StartCoroutine(ReceiveDelayRoutine));
            }
        }
        if(other.TryGetComponent<Customer>(out var customer))
        {
            var inventoryManager = customer.GetComponentInChildren<InventoryManager>();
            if (!_giveItemDictionary.ContainsKey(customer.GetInstanceID().ToString()))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, customer, customer.GetInstanceID().ToString());
                _giveItemDictionary.Add(customer.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
            }
        }
        
        if(other.TryGetComponent<Helper>(out var helper))
        {
            var inventoryManager = helper.GetComponentInChildren<InventoryManager>();

            if (!_receiveItemDictionary.ContainsKey(helper.GetInstanceID().ToString()))
            {
                ReceiveDelayRoutine = ReceiveDelay(inventoryManager, helper, helper.GetInstanceID().ToString());
                _receiveItemDictionary.Add(helper.GetInstanceID().ToString(), StartCoroutine(ReceiveDelayRoutine));
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_receiveItemDictionary.TryGetValue(player.GetInstanceID().ToString(), out Coroutine currentCoroutine))
            {
                _receiveItemDictionary.Remove(player.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
            }
        }

        if (other.TryGetComponent<Customer>(out var customer))
        {
            if (_giveItemDictionary.TryGetValue(customer.GetInstanceID().ToString(), out Coroutine currentCoroutine))
            {
                _giveItemDictionary.Remove(customer.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
            }
        }
        
        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (_receiveItemDictionary.TryGetValue(helper.GetInstanceID().ToString(), out Coroutine currentCoroutine))
            {
                _receiveItemDictionary.Remove(helper.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
            }
        }
    }

    protected override IEnumerator ReceiveDelay(InventoryManager inventoryManager, NPC npc, string entityName)
    {
        while (true)
        {
            if (_receiveItemDictionary.TryGetValue(entityName, out Coroutine currentCoroutine))
            {
                yield return new WaitForSeconds(ItemDistributeDelay);
                
                var item = TryReceiveItem(inventoryManager, npc, this);
                if (item != null)
                {
                    ReceiveItem(inventoryManager, item, ItemContains, this);
                }
                else
                {
                    _receiveItemDictionary.Remove(entityName);

                    StopCoroutine(currentCoroutine);
                }
                isFull = ItemContains.Count >= MaxCapacity;
            }
            yield return null;
        }
    }

    protected override IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc, string entityName)
    {
        while (true)
        {
            if (_giveItemDictionary.TryGetValue(entityName, out Coroutine currentCoroutine) && _receiveItemDictionary.IsEmpty()) //check if empty
            {
                yield return new WaitForSeconds(ItemDistributeDelay);
                
                Debug.Log("Try give item for " + entityName); 
                
                TryGiveItem(inventoryManager, npc);
                
                isFull = ItemContains.Count >= MaxCapacity;
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
