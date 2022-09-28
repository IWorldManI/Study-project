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
            GiveItem(inventoryManager);
        }
   }

   private void GiveItem(InventoryManager im)
   {
       var item = ItemContains[0].GetComponent<Ingredient>();
       ItemContains[0].transform.parent = im.transform;
       
       base.Move(item,this,im,im._ingredientList.Last().transform.localPosition + new Vector3(0, 1, 0));
   }

   void Foo()
   {
       var item = ItemContains[0].GetComponent<Ingredient>();
       //im.AddToDictionary(item);
       ItemContains.RemoveAt(0);
   }
}
