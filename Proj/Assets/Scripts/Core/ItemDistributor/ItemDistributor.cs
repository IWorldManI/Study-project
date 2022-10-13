using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ItemDistributor : MonoBehaviour
{
    protected List<Ingredient> ItemContains = new List<Ingredient>();
    protected List<Ingredient> ItemProduction = new List<Ingredient>();
    protected PlaceHolderForItems Holder;
    
    protected int MaxCapacity;
    protected readonly List<Transform> ItemPlace = new List<Transform>();

    protected Type StandType;
    
    protected IEnumerator DelayRoutine;

    protected virtual void Start()
    {
        
    }

    protected void GiveItem(InventoryManager inventoryManager, Ingredient item, List<Ingredient> list)
    {
        if (item != null && list.Count > 0)
        {
            var index = list.IndexOf(item);
            list.RemoveAt(index);
            
            item.transform.parent = inventoryManager.transform;

            var position = new Vector3(0, inventoryManager._ingredientList.Count + 1, 0);
            Give(item, this, inventoryManager, position);
        }
    }

    protected void ReceiveItem(InventoryManager inventoryManager,Ingredient item, List<Ingredient> list)
    {
        if (item != null && list.Count < MaxCapacity) 
        {
            item.transform.parent = ItemPlace[ItemContains.Count].transform;
            Receive(item, inventoryManager, this, new Vector3(0, 0, 0));
            list.Add(item);
        }   
    }

    private void Give(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .3f).OnComplete(()=>CompleteGive(ingredient,giver,receiver));
    }

    private void Receive(Ingredient ingredient, InventoryManager giver,ItemDistributor receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .3f).OnComplete(()=>CompleteReceive(ingredient,giver,receiver));
    }
    
    protected void CompleteGive(Ingredient ingredient, ItemDistributor giver,InventoryManager receiver)
    {
        receiver.AddToDictionary(ingredient);
        Debug.Log("ItemGive complete");
    }
    protected void CompleteReceive(Ingredient ingredient, InventoryManager  giver,ItemDistributor receiver)
    {
        giver.RemoveItemFromDictionary(ingredient);
        Debug.Log("ItemReceive complete");
    }

    protected virtual IEnumerator Delay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(.4f);
    }
}
