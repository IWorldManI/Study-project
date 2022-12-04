using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entity.NPC;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ItemDistributor : MonoBehaviour
{
    protected List<Ingredient> ItemContains = new List<Ingredient>();
    protected List<Ingredient> ItemProduction = new List<Ingredient>();
    protected PlaceHolderForItems Holder;

    protected int MaxCapacity;
    protected readonly List<Transform> ItemPlace = new List<Transform>();

    protected internal Type StandType;

    protected IEnumerator ReceiveDelayRoutine;
    protected IEnumerator GiveDelayRoutine;
    
    private float _itemDistributeDuration = 0.3f;
    protected float ItemDistributeDelay;

    internal EventBus _eventBus;

    private Sequence _giveSequence;
    private Sequence _receiveSequence;

    protected bool InProcess = false;
    
    protected virtual void Start()
    {
        ItemProduction = new List<Ingredient>();
        ItemContains = new List<Ingredient>();
        ItemDistributeDelay = _itemDistributeDuration + 0.1f;
        
        _eventBus = FindObjectOfType<EventBus>();
    }
    
    protected void TryReceiveItem(InventoryManager inventoryManager, NPC npc, ItemDistributor distributor)
    {
        if (ItemContains.Count < MaxCapacity) 
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(StandType);
            ReceiveItem(inventoryManager, item, ItemContains, distributor);
        }
    }
    protected void TryGiveItem(InventoryManager inventoryManager, NPC npc)
    {
        if (StandType == inventoryManager.LookingItem && ItemContains.Count > 0 && inventoryManager._ingredientList.Count < inventoryManager.MaxCapacity)
        {
            var item = ItemContains[^1].GetComponent<Ingredient>();
            GiveItem(inventoryManager, item, ItemContains);
            
            if (npc != null)
            {
                _eventBus.OnCollect += npc.TryNextTarget;
                _eventBus.OnCollect?.Invoke(npc);
                _eventBus.OnCollect -= npc.TryNextTarget;
            }
        }
    }

    protected void GiveItem(InventoryManager inventoryManager, Ingredient ingredient, List<Ingredient> list)
    {
        if (ingredient != null && list.Count > 0)
        {
            ingredient.transform.parent = inventoryManager.transform;
            
            var index = list.IndexOf(ingredient);
            list.RemoveAt(index);
            
            var position = new Vector3(0, inventoryManager._ingredientList.Count + 1, 0);
            Give(ingredient, this, inventoryManager, position, list);
            inventoryManager.AddToDictionary(ingredient);
        }
    }

    protected void ReceiveItem(InventoryManager inventoryManager, Ingredient ingredient, List<Ingredient> list, ItemDistributor distributor)
    {
        if (ingredient != null && list.Count < MaxCapacity) 
        {
            if(ItemPlace[list.Count].transform.childCount <= 0)
            {
                //Debug.Log("Everything okay");
                ingredient.transform.parent = ItemPlace[list.Count].transform;
            }
            else 
            {
                //Debug.Log("Find empty");
                for (int i = ItemPlace.Count - 1; i >= 0; i--)
                {
                    if (ItemPlace[i].transform.childCount <= 0)
                    {
                        ingredient.transform.parent = ItemPlace[i].transform;
                    }
                }
                //item.transform.parent = list.Count > 0 ? ItemPlace[list.Count - 1].transform : ItemPlace[list.Count].transform;
            }
            Receive(ingredient, inventoryManager, distributor, Vector3.zero, list);
            list.Add(ingredient);
        }   
    }

    private void Give(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 target, List<Ingredient> list)
    {
        ingredient.transform.DOLocalRotate(Vector3.zero, _itemDistributeDuration);
        _giveSequence = ingredient.transform.DOLocalJump(target, 1f, 1, _itemDistributeDuration).OnComplete(() => CompleteGive(ingredient, giver, receiver, list));
    }

    private void Receive(Ingredient ingredient, InventoryManager giver, ItemDistributor receiver, Vector3 target, List<Ingredient> list)
    {
        ingredient.transform.DOLocalRotate(Vector3.zero,_itemDistributeDuration);
        _receiveSequence = ingredient.transform.DOLocalJump(target, 1f, 1, _itemDistributeDuration).OnComplete(()=>CompleteReceive(ingredient,giver,receiver,list));
        InProcess = true;
    }

    private void CompleteGive(Ingredient ingredient, ItemDistributor giver, InventoryManager receiver, List<Ingredient> list)
    {
        //receiver.AddToDictionary(ingredient);
        //Debug.Log("ItemGive complete");
    }

    private void CompleteReceive(Ingredient ingredient, InventoryManager  giver, ItemDistributor receiver, List<Ingredient> list)
    {
        InProcess = false;
        //list.Add(ingredient);
        //_eventBus.OnCollectStand?.Invoke(receiver);
        
        Debug.Log("ItemReceive complete");
    }
    
    protected  virtual void OnCollectAction(ItemDistributor distributor)
    {
       
    }
}
