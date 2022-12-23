using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Entity.NPC;
using ModestTree;
using UnityEngine;

public class CashierStand : ItemDistributor, ISubject
{
    private int _customerCount = 0;
    
    private MoneyPool _moneyPool;

    private List<Customer> _customers = new List<Customer>();
    private List<PooledShape> _moneyList = new List<PooledShape>();
    
    private readonly Dictionary<string, Coroutine> _receiveItemDictionary = new Dictionary<string, Coroutine>();
    private readonly Dictionary<string, Coroutine> _processCustomer = new Dictionary<string, Coroutine>();

    private Sequence _moneyAnimation;

    private IEnumerator _giveMoney;
    private SpawnPointForProcessItems _moneyHolder;
    private readonly List<Transform> _placeForMoney = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();
            _customers = new List<Customer>();

            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }

            _moneyHolder = GetComponentInChildren<SpawnPointForProcessItems>();
            foreach (Transform child in _moneyHolder.transform)
            {
                _placeForMoney.Add(child);
            }
            
            MaxCapacity = ItemPlace.Count;
            _moneyPool = FindObjectOfType<MoneyPool>();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Customer>(out var npc))
        {
            var inventoryManager = npc.GetComponentInChildren<InventoryManager>();
            if (!_receiveItemDictionary.ContainsKey(npc.GetInstanceID().ToString()))
            {
                ReceiveDelayRoutine = ReceiveDelay(inventoryManager, npc,  npc.GetInstanceID().ToString());
                _receiveItemDictionary.Add(npc.GetInstanceID().ToString(), StartCoroutine(ReceiveDelayRoutine));
                Debug.Log("Routine started " + npc.name + npc.GetInstanceID());
                _customerCount++;
            }
        }

        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (!_processCustomer.ContainsKey(player.GetInstanceID().ToString()))
            {
                GiveDelayRoutine = GiveDelay(npc,  player.GetInstanceID().ToString(), player);
                _giveMoney = TryGiveMoneyToPlayer(player);
                _processCustomer.Add(player.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                _processCustomer.Add(player.GetInstanceID().ToString() + "moneyRoutine", StartCoroutine(_giveMoney));
            }
        }

        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (!_processCustomer.ContainsKey(helper.GetInstanceID().ToString()))
            {
                GiveDelayRoutine = GiveDelay(npc,  helper.GetInstanceID().ToString(), null);
                _processCustomer.Add(helper.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_processCustomer.TryGetValue(player.GetInstanceID().ToString(), out Coroutine currentCoroutine));
            {
                _processCustomer.Remove(player.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
                //Debug.Log("Routine stopped player" + player.GetInstanceID().ToString());
            }
            if(_processCustomer.TryGetValue(player.GetInstanceID().ToString() + "moneyRoutine", out Coroutine currentMoneyCoroutine))
            {
                _processCustomer.Remove(player.GetInstanceID().ToString() + "moneyRoutine");

                StopCoroutine(currentMoneyCoroutine);
            }
        }

        if (other.TryGetComponent<Customer>(out var npc))
        {
            if (_receiveItemDictionary.TryGetValue(npc.GetInstanceID().ToString(), out Coroutine currentCoroutine))
            {
                _receiveItemDictionary.Remove(npc.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
                Debug.Log("Routine stopped npc" + npc.GetInstanceID());
                _customerCount--;
            }
        }

        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (_processCustomer.TryGetValue(helper.GetInstanceID().ToString(), out Coroutine currentCoroutine))
            {
                _processCustomer.Remove(helper.GetInstanceID().ToString());

                StopCoroutine(currentCoroutine);
            }
        }
    }
    
    private void SellItems(InventoryManager inventoryManager, CharacterMoveAndRotate player)
    {
        PutMoneyToStand(inventoryManager, player);
        
        while (ItemContains.Count > 0)
        {
            var item = ItemContains.LastOrDefault();
            GiveItem(inventoryManager, item, ItemContains);
        }
        
        _customers[0].MovingToExit();

        Detach(_customers[0]);

        if(!_customers.IsEmpty())
            _customers[0].UpdateObserver(this);
    }

    private void PutMoneyToStand(InventoryManager inventoryManager, CharacterMoveAndRotate player)
    {
        foreach (var item in ItemContains)
        {
            var money =_moneyPool.Spawn(inventoryManager.transform.position);
            _moneyList.Add(money);

            money.transform.DOLocalRotate(Vector3.zero, _itemDistributeDuration);
            _moneyAnimation = money.transform.DOLocalJump(Vector3.zero, 1f, 1, .7f).SetEase(Ease.InOutQuad).OnComplete(() => CompletePutMoneyToStand(money, player));

            if (_placeForMoney.Count >= _moneyList.Count) 
            {
                money.transform.parent = _placeForMoney[_moneyList.Count - 1];
            }
            else
            {
                money.transform.parent = _placeForMoney[^1];
                Debug.Log("!!!MAX!!!");
            }
        }
    }
    private void CompletePutMoneyToStand(PooledShape money, CharacterMoveAndRotate player)
    { 
        
    }

    private IEnumerator TryGiveMoneyToPlayer(CharacterMoveAndRotate player)
    {
        while (true)
        {
             if (player != null && _moneyList.Count > 0 && !_moneyAnimation.IsPlaying()) 
             {
                 var money = _moneyList[^1];
                            
                 money.transform.DOLocalRotate(Vector3.zero, _itemDistributeDuration);
                 money.transform.DOLocalJump(Vector3.zero, 1f, 1, .3f).OnComplete(() => CompleteMoneyGive(money));
                 money.transform.parent = player.transform;
                 _moneyList.Remove(money);
                 yield return new WaitForSeconds(0.1f); //mb 0.05
             }
            
             yield return null;
             Debug.Log("Routine working!!!!!");
        }
       
    }
    private void TryGiveMoneyPlayer(CharacterMoveAndRotate player)
    {
        
    }

    private void CompleteMoneyGive(PooledShape money)
    {
        _moneyPool.KillShape(money);
    }
   
    private IEnumerator ReceiveDelay(InventoryManager inventoryManager, Customer npc, string entityName)
    {
        while (true)
        {
            if (ItemContains.Count < MaxCapacity && _receiveItemDictionary.TryGetValue(npc.GetInstanceID().ToString(), out Coroutine currentCoroutine))  
            {
                var type = inventoryManager._ingredientList.LastOrDefault()?.GetType();
                var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(type);
                if (item != null)
                {
                    ReceiveItem(inventoryManager, item, ItemContains, this);
                }
                else
                {
                    _receiveItemDictionary.Remove(npc.GetInstanceID().ToString());
            
                    StopCoroutine(currentCoroutine);
                    npc.WaitForPay = true;
                }
                yield return new WaitForSeconds(ItemDistributeDelay);
            }
            yield return null;
        }
    }

    private  IEnumerator GiveDelay(NPC npc, string entityName, CharacterMoveAndRotate player)
    {
        while (true)
        {
            if (_processCustomer.TryGetValue(entityName, out Coroutine currentCoroutine)) 
            { 
                TryGiveMoneyPlayer(player);
                yield return new WaitForSeconds(ItemDistributeDelay);
                
                Debug.Log("Try sell items " + entityName); 
                
                if(!_customers.IsEmpty() && _customers[0].WaitForPay)
                {
                    var inventoryManager = _customers[0].GetComponentInChildren<InventoryManager>();
                    SellItems(inventoryManager, player);
                    Debug.Log("Money spawn");
                }
                else
                {
                    Debug.Log("Customer putting items, wait");
                }
            }
            yield return null;
        }
    }

    public Customer GetLastCustomer()
    {
        if (!_customers.IsEmpty())
        {
            var lastCustomer = _customers.LastOrDefault();
            return lastCustomer;
        }
        else
            return null;
    }
    
    public void Attach(Customer observer)
    {
        if(!_customers.Contains(observer))
        {
            _customers.Add(observer);
            Debug.Log(observer + " Added");
        }
    }

    public void Detach(Customer observer)
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
