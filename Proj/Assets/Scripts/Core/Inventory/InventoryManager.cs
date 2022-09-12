using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<Type, List<Ingredient>> _ingredientListDictionary = new Dictionary<Type, List<Ingredient>>();
 
    [SerializeField] private List<Ingredient> _ingredientList = new List<Ingredient>();
 
    private void Awake()
    {
        if (_ingredientList.Count >= 0)
        {
            FindAllItemsInListAndSort();
        }
    }
    
    private void Start()
    {
        //SearchAllAndAddToDictionary();
    }
    
    private void SearchAllAndAddToDictionary()
    {
        foreach (Ingredient item in _ingredientList)
        {
            AddToDictionary(item);
        }
    }
 
    public void AddToDictionary(Ingredient item)
    {
        _ingredientList.Add(item);
        var type = item.GetType();

        if(!_ingredientListDictionary.ContainsKey(type))
        {
            _ingredientListDictionary.Add(type, new List<Ingredient>(){item});
        }
        else
        {
            _ingredientListDictionary[type].Add(item);
        }
        
        if (item.TryGetComponent<Wobbling>(out Wobbling _wobbling))
        {
            SetProperties(_wobbling);
        }
        
        //Debug.Log(item.GetType());
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var itemToRemove = _ingredientList.GetType();

            if (!_ingredientListDictionary.ContainsKey(itemToRemove))
            {
                RemoveItemFromDictionary(_ingredientList[0]);
            }
        }
    }
    
    public void RemoveItemFromDictionary(Ingredient item)
    {
        Debug.Log(_ingredientList.Count);
        
        _ingredientList.Remove(item);
        var itemToRemove = item.GetType();

        if (!_ingredientListDictionary.ContainsKey(itemToRemove))
        {
            _ingredientListDictionary[itemToRemove].Remove(item);
        }
        
        Destroy(item.gameObject);
        //_ingredientList.RemoveAll(item => item == null);
        
        
        SortItemsInListAfterRemovalOfAny();
        
        Debug.Log("Item removed" + item);
        Debug.Log(_ingredientList.Count);
    }
    
    private void FindAllItemsInListAndSort()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent<Wobbling>(out Wobbling _wobbling))
            {
                if (!_wobbling.OnTook)
                {
                    var item = child.GetComponent<Ingredient>();
                    AddToDictionary(item);
                    SetProperties(_wobbling);
                }
            }
        }
    }
    
    private void SetProperties(Wobbling _wobbling)
    {
        _wobbling.OnTook = true;
        //var index = _ingredientList.FindIndex(i => i == _wobbling.GetComponent<Ingredient>());
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
        foreach (var t in _ingredientList)
        {
            var test = t.GetComponent<Wobbling>();
            test.OnTook = true;
            var socket = indx <= 0 ? transform : _ingredientList[indx-1].transform;;
            test.pivot = socket;
            test.transform.position = new Vector3(socket.transform.position.x, socket.transform.position.y + 1f, socket.transform.position.z);
            test.stiffness = 399;
            test.conservation = 0.6f;
            test.Initialize();
            indx++;
        }
    }
}