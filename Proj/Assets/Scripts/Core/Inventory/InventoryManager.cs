using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private readonly Dictionary<Type, List<Ingredient>> _ingredientDictionary = new Dictionary<Type, List<Ingredient>>();
    public List<Ingredient> _ingredientList = new List<Ingredient>();

    public Type LookingItem;

    private void Awake()
    {
        if (_ingredientList.Count >= 0)
        {
            SearchAllAndAddToDictionary();
        }
    }
   
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
        }
    }

    public Ingredient ItemGiveRequest(Type type)
    {
        var item = _ingredientList.LastOrDefault(x => x.GetType() == type);
        var indexList = _ingredientList.IndexOf(item);

        if (indexList >= 0)
        {
            RemoveItemFromDictionary(_ingredientList[indexList]);
            //Debug.Log(indexList, item);
        }
        else 
            Debug.Log("Dont have current type item");

        return item;
    }

    private void SearchAllAndAddToDictionary()
    {
        foreach (Transform child in transform)
        {
            var item = child.GetComponent<Ingredient>();
            AddToDictionary(item);
        }

        SortItemsInList();
    }
 
    public void AddToDictionary(Ingredient item)
    {
        _ingredientList.Add(item);
        var type = item.GetType();

        if(!_ingredientDictionary.ContainsKey(type))
        {
            _ingredientDictionary.Add(type, new List<Ingredient>(){item});
        }
        else
        {
            _ingredientDictionary[type].Add(item);
        }
    }
    
    
    public void RemoveItemFromDictionary(Ingredient item)
    {
        //Debug.Log(_ingredientList.Count + " items contains before remove");
        
        var type = item.GetType();

        if (_ingredientDictionary.TryGetValue(type, out var list))
        {
            list.Remove(item);
            //Debug.Log("Item removed " + type);
        }
        
        var indexList = _ingredientList.IndexOf(item);
        _ingredientList.RemoveAt(indexList);
        
        item.transform.parent = null;
        
        SortItemsInList();
        
        //Debug.Log(_ingredientList.Count + " item contains after remove");
    }

    private void SortItemsInList()
    {
        int indx = 0;
        foreach (var ingredient in _ingredientList)
        {
            var socket = indx <= 0 ? transform : _ingredientList[indx-1].transform;;
            ingredient.transform.position = new Vector3(socket.transform.position.x, socket.transform.position.y + 1f, socket.transform.position.z);
            indx++;
        }
    }
}