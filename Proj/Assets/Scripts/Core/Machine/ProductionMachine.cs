using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ProductionMachine : ItemDistributor
{
    [SerializeField] private KetchupPool productPool; //testing, there should be a pool
    [SerializeField] private TomatoesPool containsPool;

    [SerializeField] private Transform spawnPoint;
    private IEnumerator _productRoutine;
    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();
            ItemProduction = new List<Ingredient>();

            Holder = GetComponentInChildren<PlaceHolderForItems>();
            spawnPoint = GetComponentInChildren<SpawnPointForProcessItems>().transform;
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            MaxCapacity = ItemPlace.Count;
            StandType = typeof(Tomatoes);

            productPool = FindObjectOfType<KetchupPool>();
            containsPool = FindObjectOfType<TomatoesPool>();
        }
    }
    
    protected override void OnCollectAction(ItemDistributor distributor)
    {
        Debug.Log(name + " was collect item");
    }

    private IEnumerator Process(Type type)
    {
        while (true)
        { 
            yield return new WaitForSeconds(2f);
                    var itemForProcessing = ItemContains.LastOrDefault(x => x.GetType() == type);
                    if (itemForProcessing != null) 
                    {
                        CreateProduct(itemForProcessing);
                        Debug.Log("Start again routine cuz have item to product");
                    }else
                    {
                        if (_productRoutine != null)
                        {
                            StopCoroutine(_productRoutine);
                            _productRoutine = null;
                        }
                    }
        }
    }

    private void CreateProduct(Ingredient itemForProcessing)
    {
        var index = ItemContains.IndexOf(itemForProcessing);
        ItemContains.RemoveAt(index);
        containsPool.KillShape(itemForProcessing.GetComponent<PooledShape>());
                        
        var item = productPool.Spawn(transform.position + new Vector3(0, ItemProduction.Count, 0));
                        
        item.transform.parent = transform;
        Vector3 targetPosition = spawnPoint.localPosition + new Vector3(0, ItemProduction.Count, 0);
        item.transform.DOLocalJump(targetPosition, 1f, 1, 0.4f).OnComplete(() => CompleteItemCreate(item));
    }

    private void CompleteItemCreate(PooledShape item)
    {
        ItemProduction.Add(item.GetComponent<Ingredient>());
    }
    
    private IEnumerator Delay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(ItemDistributeDelay);
       
        if (ItemContains.Count < MaxCapacity)
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(StandType);
            ReceiveItem(inventoryManager,item, ItemContains, this);
        }
            
        if (ItemProduction.Count > 0 && inventoryManager._ingredientList.Count< inventoryManager.MaxCapacity) 
        {
            var item = ItemProduction.LastOrDefault(x => x.GetType() == typeof(Ketchup));
            GiveItem(inventoryManager, item, ItemProduction);
        }

        ReceiveDelayRoutine = Delay(inventoryManager);
        StartCoroutine(ReceiveDelayRoutine);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            
            ReceiveDelayRoutine = Delay(inventoryManager);
            StartCoroutine(ReceiveDelayRoutine);
            if (_productRoutine == null)
            {
                _productRoutine = Process(StandType);
                StartCoroutine(_productRoutine);
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
}
