using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<Type, List<Ingredient>> _ingredientDictionary = new Dictionary<Type, List<Ingredient>>();
    public List<Ingredient> _ingredientList = new List<Ingredient>();
 
    private void Awake()
    {
        if (_ingredientList.Count >= 0)
        {
            SearchAllAndAddToDictionary();
        }
    }
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
        }
    }

    public Ingredient ItemGiveRequest(Type type)
    {
        var item = _ingredientList.Where(x => x.GetType() == type).LastOrDefault();
        var indexList = _ingredientList.IndexOf(item);

        if (indexList >= 0)
        {
            RemoveItemFromDictionary(_ingredientList[indexList]);
            Debug.Log(indexList, item);
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
        
        if (item.TryGetComponent<Wobbling>(out Wobbling _wobbling))
        {
            SetProperties(_wobbling);
        }
        
        //Debug.Log(item.GetType());
    }
    
    
    public void RemoveItemFromDictionary(Ingredient item)
    {
        Debug.Log(_ingredientList.Count + " items contains before remove");
        
        var type = item.GetType();

        if (_ingredientDictionary.TryGetValue(type, out var list))
        {
            list.Remove(item);
            Debug.Log("Item removed " + type);
        }
        
        var indexList = _ingredientList.IndexOf(item);
        _ingredientList.RemoveAt(indexList);

        if (item.TryGetComponent<Wobbling>(out Wobbling _wobbling))
        {
            _wobbling.OnTook = false;
        }
        item.transform.parent = null;
        
        SortItemsInListAfterRemovalOfAny();
        
        Debug.Log(_ingredientList.Count + " item contains after remove");
    }
    
   
    
    private void SetProperties(Wobbling _wobbling)
    {
        _wobbling.OnTook = true;
        var socket = _ingredientList.Count <= 1 ? transform :  _ingredientList[^2].transform;
        _wobbling.pivot = socket;
        _wobbling.transform.position = new Vector3(socket.transform.position.x, socket.transform.position.y + 1f, socket.transform.position.z);
        _wobbling.stiffness = 399;
        _wobbling.conservation = 0.6f;
        _wobbling.Initialize();
    }
    
    private void SortItemsInListAfterRemovalOfAny()
    {
        int indx = 0;
        foreach (var ingredient in _ingredientList)
        {
            var item = ingredient.GetComponent<Wobbling>();
            item.OnTook = true;
            var socket = indx <= 0 ? transform : _ingredientList[indx-1].transform;;
            item.pivot = socket;
            item.transform.position = new Vector3(socket.transform.position.x, socket.transform.position.y + 1f, socket.transform.position.z);
            item.stiffness = 399;
            item.conservation = 0.6f;
            item.Initialize();
            indx++;
        }
    }
}