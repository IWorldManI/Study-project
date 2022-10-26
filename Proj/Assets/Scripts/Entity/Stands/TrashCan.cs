using System.Collections;
using System.Linq;
using UnityEngine;

public class TrashCan : ItemDistributor
{
    private IEnumerator _delayRoutine;
    
    private void Start()
    {
        base.Start();
        {
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                ItemPlace.Add(child);
            }
            
            MaxCapacity = 999;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            var inventoryManager = player.GetComponentInChildren<InventoryManager>();
            {
                _delayRoutine = Delay(inventoryManager);
                StartCoroutine(_delayRoutine);
            }
        }
    }
    private void DropInCan(InventoryManager inventoryManager)
    {
#if UNITY_EDITOR
        Debug.Log("Trashed " + name);
#endif
        var type = inventoryManager._ingredientList.LastOrDefault().GetType();
        var item = inventoryManager.GetComponentInChildren<InventoryManager>().ItemGiveRequest(type);
        ReceiveItem(inventoryManager, item, ItemContains);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            if (_delayRoutine != null) 
            {
                StopCoroutine(_delayRoutine);
            }
        }
    }

    private IEnumerator Delay(InventoryManager inventoryManager)
    {
        yield return new WaitForSeconds(.15f);
        if (inventoryManager._ingredientList.Count > 0) 
            DropInCan(inventoryManager);
        
        if (ItemContains.Count > 0)
        {
            _delayRoutine = Delay(inventoryManager);
            StartCoroutine(_delayRoutine);
        }
    }
}
