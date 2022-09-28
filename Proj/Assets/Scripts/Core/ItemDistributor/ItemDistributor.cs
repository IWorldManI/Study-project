using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemDistributor : MonoBehaviour
{
    [SerializeField]
     private InventoryManager _inventoryManager;
     
    protected  List<Ingredient> ItemContains = new List<Ingredient>();
    protected PlaceHolderForItems Holder;
    
    protected int MaxCapacity;
    protected readonly List<Transform> ItemPlace = new List<Transform>();
    
    protected InventoryManager InventoryManager { get { return _inventoryManager; } }
    
    protected virtual void Start()
    {
        
    }

    protected void Move(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver, Vector3 _target)
    {
        ingredient.transform.DOLocalJump(_target, 1f, 1, .5f).OnComplete(()=>Complete(ingredient,giver,receiver));
    }
    
    private void Complete(Ingredient ingredient,ItemDistributor giver,InventoryManager receiver)
    {
        Debug.Log("CompleteAnima");
    }
}
