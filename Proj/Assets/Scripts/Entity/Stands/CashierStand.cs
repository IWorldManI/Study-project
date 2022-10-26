using Entity.NPC;
using UnityEngine;

public class CashierStand : ItemDistributor
{
    private MoneyPool _moneyPool;
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
            _moneyPool = FindObjectOfType<MoneyPool>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out var npc))
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            foreach (var ingredient in inventoryManager._ingredientList)
            {
                var item = ingredient.GetComponent<Ingredient>();
                ReceiveItem(inventoryManager, item, ItemContains);
            }
            //only test
            npc.OnCollect += npc.OrderNext;
            npc.OnCollect?.Invoke();
            npc.OnCollect -= npc.OrderNext;
        }
        
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            //sell a product to a customer
            _moneyPool.Spawn(transform.position+new Vector3(0,10,0), ItemContains.Count);
            Debug.Log("Money spawn");
        }
    }
}
