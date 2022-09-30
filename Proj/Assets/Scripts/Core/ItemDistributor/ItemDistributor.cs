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

    protected virtual void Start()
    {
        
    }

    protected void GiveItem(InventoryManager inventoryManager, Ingredient item)
    {
        item.transform.parent = inventoryManager.transform;
       
        Give(item,this,inventoryManager,inventoryManager._ingredientList.Last().transform.localPosition + new Vector3(0, 1, 0)); ;
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

    protected void Give(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .5f).OnComplete(()=>CompleteGive(ingredient,giver,receiver));
    }
    protected void Receive(Ingredient ingredient,InventoryManager giver,ItemDistributor receiver, Vector3 _target)
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
