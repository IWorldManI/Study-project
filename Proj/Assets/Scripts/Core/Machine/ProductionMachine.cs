using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ProductionMachine : ItemDistributor
{
    [SerializeField] private KetchupPool productPool; //testing, there should be a pool

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
            StandType = typeof(Tomatoes);
            
            StartCoroutine(Process(StandType));
            productPool = GetComponent<KetchupPool>();
        }
    }

    private IEnumerator Process(Type type)
    {
        while (true)
        { 
            yield return new WaitForSeconds(2f);
                    var itemForProcessing = ItemContains.LastOrDefault(x => x.GetType() == type);
                    if (itemForProcessing != null) 
                    {
                        var index = ItemContains.IndexOf(itemForProcessing);
                        ItemContains.RemoveAt(index);
                        Destroy(itemForProcessing.gameObject);
                        var item = productPool.Spawn(transform.position + new Vector3(0, ItemProduction.Count, 0), 1);
                        
                        item.transform.parent = transform;
                        ItemProduction.Add(item.GetComponent<Ingredient>());
                        
                        Debug.Log("Start again routine cuz have item to product");
                    }
        }
    }

    protected override IEnumerator Delay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(.4f);
       
        if (ItemContains.Count < MaxCapacity)
        {
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(typeof(Tomatoes));
            ReceiveItem(inventoryManager,item, ItemContains);
        }
            
        if (ItemProduction.Count > 0) 
        {
            var item = ItemProduction.LastOrDefault(x => x.GetType() == typeof(Ketchup));
            GiveItem(inventoryManager, item, ItemProduction);
        }

        DelayRoutine = Delay(inventoryManager);
        StartCoroutine(DelayRoutine);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            
            DelayRoutine = Delay(inventoryManager);
            StartCoroutine(DelayRoutine);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (DelayRoutine != null) 
            {
                StopCoroutine(DelayRoutine);
            }
        }
    }
}
