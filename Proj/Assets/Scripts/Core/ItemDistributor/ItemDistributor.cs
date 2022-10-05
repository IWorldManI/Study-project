using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ItemDistributor : MonoBehaviour
{
    protected List<Ingredient> ItemContains = new List<Ingredient>();
    protected PlaceHolderForItems Holder;
    
    protected int MaxCapacity;
    protected readonly List<Transform> ItemPlace = new List<Transform>();

    protected Type StandType;

    protected virtual void Start()
    {
        
    }

    protected void GiveItem(InventoryManager inventoryManager, Ingredient item)
    {
        if (item != null && ItemContains.Count > 0)
        {
            item.transform.parent = inventoryManager.transform;

            var position = inventoryManager._ingredientList.Count == 0
                ? new Vector3(0, 0, 0)
                : inventoryManager._ingredientList.Last().transform.localPosition; 
            Give(item, this, inventoryManager,
                position + new Vector3(0, 1, 0));
        }
    }

    protected void ReceiveItem(InventoryManager inventoryManager,Ingredient item)
    {
        if (item != null && ItemContains.Count <= MaxCapacity) 
        {
            item.transform.parent = ItemPlace[ItemContains.Count].transform;
            Receive(item, inventoryManager, this, new Vector3(0, 0, 0));
            ItemContains.Add(item);
        }   
    }

    private void Give(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .5f).OnComplete(()=>CompleteGive(ingredient,giver,receiver));
    }

    private void Receive(Ingredient ingredient,InventoryManager giver,ItemDistributor receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .5f).OnComplete(()=>CompleteReceive(ingredient,giver,receiver));
    }
    
    private void CompleteGive(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver)
    {
        receiver.AddToDictionary(ingredient);
        Debug.Log("ItemGive");
    }
    private void CompleteReceive(Ingredient ingredient,InventoryManager  giver,ItemDistributor receiver)
    {
        giver.RemoveItemFromDictionary(ingredient);
        Debug.Log("ItemReceive");
    }
}
