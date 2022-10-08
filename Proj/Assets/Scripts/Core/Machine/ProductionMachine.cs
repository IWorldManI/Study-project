using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ProductionMachine : ItemDistributor
{
    [SerializeField] private GameObject productPrefab;
    
    private void Start()
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
        }
    }

    private IEnumerator Product(Type type)
    {
        yield return new WaitForSeconds(10f);
        var itemForProcessing = ItemContains.LastOrDefault(x => x.GetType() == type);
        if (itemForProcessing != null) 
        {
            var index = ItemContains.IndexOf(itemForProcessing);
            ItemContains.RemoveAt(index);
            Destroy(itemForProcessing.gameObject);
            var item = Instantiate(productPrefab,transform.position,Quaternion.identity);
            item.transform.parent = transform;
            ItemContains.Add(item.GetComponent<Ingredient>());
            
            Debug.Log("Start again routine cuz have item to production");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            
            if (ItemContains.Count < MaxCapacity)
            {
                var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(typeof(Tomatoes));
                ReceiveItem(inventoryManager,item);
                StartCoroutine(Product(typeof(Tomatoes)));
            }
            
            if (ItemContains.Count > 0) 
            {
                var item = ItemContains.LastOrDefault(x => x.GetType() == typeof(Ketchup));
                GiveItem(inventoryManager, item, this);
            }
        }
    }
}
