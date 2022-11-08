using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.NPC;
using UnityEngine;

public class CashierStand : ItemDistributor
{
    private MoneyPool _moneyPool;
    
    private readonly Dictionary<NPC, Coroutine> _receiveItemDictionary = new Dictionary<NPC, Coroutine>();
    private readonly Dictionary<NPC, Coroutine> _giveItemDictionary = new Dictionary<NPC, Coroutine>(); //maybe helper class
    
    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();
            
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            
            MaxCapacity = 999;
            _moneyPool = FindObjectOfType<MoneyPool>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out var npc))
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            if (!_receiveItemDictionary.ContainsKey(npc))
            {
                GiveDelayRoutine = ReceiveDelay(inventoryManager, npc);
                _receiveItemDictionary.Add(npc, StartCoroutine(GiveDelayRoutine));
                Debug.Log("Routine started npc");
            }
            //only test
            npc.OnCollect += npc.TryOrderNext;
            npc.OnCollect?.Invoke(npc);
            npc.OnCollect -= npc.TryOrderNext;
        }

        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            //sell a product to a customer
            _moneyPool.Spawn(transform.position+new Vector3(0,10,0), ItemContains.Count);
            Debug.Log("Money spawn");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (ReceiveDelayRoutine != null) 
            {
                StopCoroutine(ReceiveDelayRoutine);
                Debug.Log("Routine stopped player");
            }
        }

        if (other.TryGetComponent<NPC>(out var npc))
        {
            if (_receiveItemDictionary.TryGetValue(npc, out Coroutine rCoroutine))
            {
                _receiveItemDictionary.Remove(npc);

                StopCoroutine(rCoroutine);
                Debug.Log("Routine stopped npc" + npc.name);
            }
        }
    }
    
    private IEnumerator ReceiveDelay(InventoryManager inventoryManager, NPC npc)
    {
        yield return new WaitForSeconds(ItemDistributeDelay);

        if (ItemContains.Count < MaxCapacity && inventoryManager._ingredientList.Count > 0) 
        {
            var type = inventoryManager._ingredientList.LastOrDefault().GetType();
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(type);
            ReceiveItem(inventoryManager, item, ItemContains);
        }
        if(inventoryManager._ingredientList.Count>0)
        {
            GiveDelayRoutine = ReceiveDelay(inventoryManager, npc);
            StartCoroutine(GiveDelayRoutine);
        }
    }
}
