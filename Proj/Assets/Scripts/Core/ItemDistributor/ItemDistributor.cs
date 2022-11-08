using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entity.NPC;
using UnityEngine;

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
    
    protected virtual void Start()
    {
        ItemProduction = new List<Ingredient>();
        ItemContains = new List<Ingredient>();
        ItemDistributeDelay = _itemDistributeDuration + 0.1f;
    }
    
    protected void TryReceiveItem(InventoryManager inventoryManager, NPC npc)
    {
        if (ItemContains.Count < MaxCapacity) 
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(StandType);
            ReceiveItem(inventoryManager, item, ItemContains);
        }
    }
    protected void TryGiveItem(InventoryManager inventoryManager, NPC npc)
    {
        if (StandType == inventoryManager.LookingItem && ItemContains.Count > 0)
        {
            var item = ItemContains[^1].GetComponent<Ingredient>();
            GiveItem(inventoryManager, item, ItemContains);
            if (npc != null)
            {
                npc.OnCollect += npc.TryOrderNext;
                npc.OnCollect?.Invoke(npc);
                npc.OnCollect -= npc.TryOrderNext;
                
                //StopCoroutine(GiveDelayRoutine);
            }
        }
    }

    protected void GiveItem(InventoryManager inventoryManager, Ingredient item, List<Ingredient> list)
    {
        if (item != null && list.Count > 0)
        {
            item.transform.parent = inventoryManager.transform;
            
            var index = list.IndexOf(item);
            list.RemoveAt(index);
            
            var position = new Vector3(0, inventoryManager._ingredientList.Count + 1, 0);
            Give(item, this, inventoryManager, position, list);
        }
    }

    protected void ReceiveItem(InventoryManager inventoryManager,Ingredient item, List<Ingredient> list)
    {
        if (item != null && list.Count < MaxCapacity) 
        {
            if(ItemPlace[list.Count].transform.childCount <= 0)
            {
                Debug.Log("Everything okay");
                item.transform.parent = ItemPlace[list.Count].transform;
            }
            else 
            {
                Debug.Log("Find empty");
                item.transform.parent = list.Count > 0 ? ItemPlace[list.Count - 1].transform : ItemPlace[list.Count].transform;
            }
            Receive(item, inventoryManager, this, Vector3.zero, list);
        }   
    }

    private void Give(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 target, List<Ingredient> list)
    {
        ingredient.transform.DOLocalRotate(Vector3.zero,_itemDistributeDuration);
        ingredient.transform.DOLocalJump(target, 1f, 1, _itemDistributeDuration).OnComplete(()=>CompleteGive(ingredient,giver,receiver, list));
    }

    private void Receive(Ingredient ingredient, InventoryManager giver,ItemDistributor receiver, Vector3 target, List<Ingredient> list)
    {
        ingredient.transform.DOLocalRotate(Vector3.zero,_itemDistributeDuration);
        ingredient.transform.DOLocalJump(target, 1f, 1, _itemDistributeDuration).OnComplete(()=>CompleteReceive(ingredient,giver,receiver,list));
    }

    private void CompleteGive(Ingredient ingredient, ItemDistributor giver, InventoryManager receiver, List<Ingredient> list)
    {
        receiver.AddToDictionary(ingredient);
        //Debug.Log("ItemGive complete");
    }

    private void CompleteReceive(Ingredient ingredient, InventoryManager  giver, ItemDistributor receiver, List<Ingredient> list)
    {
        list.Add(ingredient);
        giver.RemoveItemFromDictionary(ingredient);
        //Debug.Log("ItemReceive complete");
    }

    
}
