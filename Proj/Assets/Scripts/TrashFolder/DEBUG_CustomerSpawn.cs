using System.Collections;
using Core.Factory;
using Entity.NPC;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DEBUG_CustomerSpawn : MonoBehaviour
{
    [SerializeField] private CustomerFactory _customerFactory;
    [SerializeField] private CustomersPool _customersPool;
    [SerializeField] private int maxCustomers, customersCounter;
    
    private UIPool _uiPool;
    
    private TomatoesPool _tomatoesPool;
    private MilkPool _milkPool;
    private KetchupPool _ketchupPool;
    void Start()
    {
        _uiPool = FindObjectOfType<UIPool>();

        _tomatoesPool = FindObjectOfType<TomatoesPool>();
        _milkPool = FindObjectOfType<MilkPool>();
        _ketchupPool = FindObjectOfType<KetchupPool>();
        
        _customerFactory = FindObjectOfType<CustomerFactory>();
        _customersPool = FindObjectOfType<CustomersPool>();
        maxCustomers = 6;
        StartCoroutine(TrySpawnCustomers());
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.L)) //performance test 
        {
            var prefab = _customerFactory.CreateNewInstance();
        }
        if (Input.GetKeyDown(KeyCode.K)) //solo spawn test 
        {
            var prefab = _customerFactory.CreateNewInstance();
        }
    }
    
    private IEnumerator TrySpawnCustomers()
    {
        while (true)
        {
            if (customersCounter < maxCustomers)
            {
                yield return new WaitForSeconds(1f); 
                
                //var prefab = _customerFactory.CreateNewInstance();
                var prefab = _customersPool.Spawn(transform.position + Random.insideUnitSphere * 10);
                var customerUI = _uiPool.Spawn(transform.position);
                customerUI.GetComponent<UI2World>()._target = prefab;
                prefab.GetComponent<Customer>().UI2World = customerUI.GetComponent<UI2World>();
                customersCounter++;
            }
            yield return null;
        }
    }

    private void ReturnCustomerToPool(PooledShape customer)
    {
        customersCounter--;
        _customersPool.KillShape(customer);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Customer>(out var customer) && customer.WaitForPay)
        {
            ReturnItemsFromInventoryToPool(customer.GetComponentInChildren<InventoryManager>());
            ReturnCustomerToPool(customer.GetComponent<PooledShape>());
        }
    }

    private void ReturnItemsFromInventoryToPool(InventoryManager inventoryManager)
    {
        for (var i = inventoryManager._ingredientList.Count - 1; i >= 0; i--)
        {
            var item = inventoryManager._ingredientList[i];
            var index = inventoryManager._ingredientList.IndexOf(item);
            if (item.TryGetComponent<Tomatoes>(out var tomatoes))
            {
                _tomatoesPool.KillShape(tomatoes.GetComponent<PooledShape>());
            }

            if (item.TryGetComponent<Milk>(out var milk))
            {
                _milkPool.KillShape(milk.GetComponent<PooledShape>());
            }

            if (item.TryGetComponent<Ketchup>(out var ketchup))
            {
                _ketchupPool.KillShape(ketchup.GetComponent<PooledShape>());
            }

            inventoryManager._ingredientList.RemoveAt(index);
        }
    }
}
