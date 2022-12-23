using System.Collections;
using System.Collections.Generic;
using Entity.NPC;
using UnityEngine;

public class ItemGiver : ItemDistributor, IEnumTypes
{
    //private readonly Dictionary<string, Coroutine> _receiveItemDictionary = new Dictionary<string, Coroutine>();
    private readonly Dictionary<string, Coroutine> _giveItemDictionary = new Dictionary<string, Coroutine>(); 
    
    private TomatoesPool _tomatoesPool;
    private MilkPool _milkPool;
    private KetchupPool _ketchupPool;
    
    [SerializeField] private IEnumTypes.ItemTypes selectedTypeOfPool;
    
    protected override void Start()
    {
        base.Start();
        {
            ItemContains = new List<Ingredient>();
            
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                child.TryGetComponent<Ingredient>(out var item);
                ItemContains.Add(item);
            }
            MaxCapacity = ItemContains.Count;
            _tomatoesPool = FindObjectOfType<TomatoesPool>();
            _milkPool = FindObjectOfType<MilkPool>();
            _ketchupPool = FindObjectOfType<KetchupPool>();
            StartCoroutine(SpawnDelay());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                if (!_giveItemDictionary.ContainsKey(player.GetInstanceID().ToString()))
                {
                    GiveDelayRoutine = GiveDelay(inventoryManager, null, player.GetInstanceID().ToString());
                    _giveItemDictionary.Add(player.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                    //Debug.Log("Routine started npc" + player.GetInstanceID().ToString() + player. name);
                }
            }
        }
        if (other.TryGetComponent<Helper>(out var helper))
        {
            var inventoryManager = helper.GetComponentInChildren<InventoryManager>();
            if (!_giveItemDictionary.ContainsKey(helper.GetInstanceID().ToString()))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, helper, helper.GetInstanceID().ToString());
                _giveItemDictionary.Add(helper.GetInstanceID().ToString(), StartCoroutine(GiveDelayRoutine));
                //Debug.Log("Routine started npc" + helper.GetInstanceID().ToString()+ helper.name);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_giveItemDictionary.TryGetValue(player.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _giveItemDictionary.Remove(player.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped helper" + player.GetInstanceID().ToString() + player.name);
            }
        }
        
        if (other.TryGetComponent<Helper>(out var helper))
        {
            if (_giveItemDictionary.TryGetValue(helper.GetInstanceID().ToString(), out Coroutine rCoroutine))
            {
                _giveItemDictionary.Remove(helper.GetInstanceID().ToString());

                StopCoroutine(rCoroutine);
                //Debug.Log("Routine stopped helper" + helper.GetInstanceID().ToString()+ helper.name);
            }
        }
    }

   
    private IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc, string entityName) 
    {
        while (true)
        {
            yield return new WaitForSeconds(ItemDistributeDelay);
            
            if (!_giveItemDictionary.ContainsKey(entityName))
            {
                GiveDelayRoutine = GiveDelay(inventoryManager, npc, entityName);
                _giveItemDictionary.Add(entityName, StartCoroutine(GiveDelayRoutine));
            }
        
            TryGiveItem(inventoryManager, npc);
            
            yield return null;
        }
    }

    private IEnumerator SpawnDelay()
    {
        while (true)
        {
            if (ItemContains.Count < MaxCapacity)
            {
                PooledShape item;
                switch (selectedTypeOfPool)
                {
                    case IEnumTypes.ItemTypes.Tomatoes:
                        item = _tomatoesPool.Spawn(transform.position + new Vector3(2,ItemContains.Count,0));
                        ItemContains.Add(item.GetComponent<Ingredient>());
                        break;
                    case IEnumTypes.ItemTypes.Milk:
                        item = _milkPool.Spawn(transform.position + new Vector3(2,ItemContains.Count,0));
                        ItemContains.Add(item.GetComponent<Ingredient>());
                        break;
                    case IEnumTypes.ItemTypes.Ketchup:
                        item = _ketchupPool.Spawn(transform.position + new Vector3(2,ItemContains.Count,0));
                        ItemContains.Add(item.GetComponent<Ingredient>());
                        break;
                    default: Debug.Log("Dont have current pool in enum");
                        break;
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
