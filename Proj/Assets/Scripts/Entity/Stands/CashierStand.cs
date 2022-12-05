using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Factory;
using Entity.NPC;
using ModestTree;
using UnityEngine;

public class CashierStand : ItemDistributor, ISubject
{
    private int _customerCount = 0;
    
    private MoneyPool _moneyPool;
    
    private readonly List<IObserver> _customers = new List<IObserver>();
    
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
            
            MaxCapacity = ItemPlace.Count;
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
                Debug.Log("Routine started " + npc.name);
                _customerCount++;
            }
           
        }

        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            //here foreach to take all items
            _customers[0].ReturnToStartPosition();
            Detach(_customers[0]); //ser exit position
            
            _customers[0].UpdateObserver(this);
            
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
                _customerCount--;
            }
        }
    }
    
    private IEnumerator ReceiveDelay(InventoryManager inventoryManager, NPC npc)
    {
        yield return new WaitForSeconds(ItemDistributeDelay);

        if (ItemContains.Count < MaxCapacity && inventoryManager._ingredientList.Count > 0 && _customerCount <= 1)  
        {
            var type = inventoryManager._ingredientList.LastOrDefault()?.GetType();
            var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(type);
            ReceiveItem(inventoryManager, item, ItemContains, this);
        }

        if (inventoryManager._ingredientList.Count > 0) 
        {
            GiveDelayRoutine = ReceiveDelay(inventoryManager, npc);
            StartCoroutine(GiveDelayRoutine);
        }
    }

    public Customer GetLastCustomer()
    {
        if (_customers.IsEmpty())
        {
            return null;
        }
        else
        {
            var lastCustomer = _customers.LastOrDefault();
            return (lastCustomer as MonoBehaviour).GetComponent<Customer>();
        }
        
    }
    
    public void Attach(IObserver observer)
    {
        if(!_customers.Contains(observer))
        {
            _customers.Add(observer);
            Debug.Log(observer + " Added");
        }
    }

    public void Detach(IObserver observer)
    {
        _customers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var customer in _customers)
        {
            customer.UpdateObserver(this);
        }
    }
}
