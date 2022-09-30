using System.Linq;
using UnityEngine;

public class ItemGiver : ItemDistributor
{
    void Start()
    {
        base.Start();
        {
            Holder = GetComponentInChildren<PlaceHolderForItems>();
            foreach (Transform child in Holder.transform)
            {
                child.TryGetComponent<Ingredient>(out var item);
                ItemContains.Add(item);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
   {
        if (other.GetComponentInChildren<InventoryManager>())
        {
            //Debug.Log("Touch");
            var inventoryManager = other.GetComponentInChildren<InventoryManager>();
            var item = ItemContains[ItemContains.Count-1].GetComponent<Ingredient>();
            base.GiveItem(inventoryManager, item);
            var indx = ItemContains.IndexOf(item);
            ItemContains.RemoveAt(indx);
        }
   }

   
}
