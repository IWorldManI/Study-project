using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<Type, object> items = new Dictionary<Type, object>();
    
    [SerializeField] private List<GameObject> _tomatoes = new List<GameObject>();
    [SerializeField] private List<GameObject> _milk = new List<GameObject>();
    
    private WobblingConnector _wobblingConnector;

    private void Start()
    {
        SearchAllAndAddToDictionary();
    }

    private void SearchAllAndAddToDictionary()
    {
        _wobblingConnector = FindObjectOfType<WobblingConnector>();

        foreach (GameObject item in _wobblingConnector._itemsInBackpack)
        {
            if (item.TryGetComponent<Tomatoes>(out Tomatoes tomatoes))
            {
                _tomatoes.Add(tomatoes.gameObject);
                
                if(!items.TryGetValue(typeof(Tomatoes),out _))
                    items.Add(typeof(Tomatoes), tomatoes.name);
                Debug.Log(tomatoes);
            } 
            if (item.TryGetComponent<Milk>(out Milk milk))
            {
                _milk.Add(milk.gameObject);
                if(!items.TryGetValue(typeof(Milk),out _))
                    items.Add(typeof(Milk), milk.name);
                Debug.Log(milk);
            }
        }
    }

    public void AddToDictionary(GameObject item)
    {
        if (item.TryGetComponent<Tomatoes>(out Tomatoes tomatoes))
        {
            _tomatoes.Add(tomatoes.gameObject);
            items[typeof(Tomatoes)] = tomatoes;
            Debug.Log(tomatoes);
        } 
        if (item.TryGetComponent<Milk>(out Milk milk))
        {
            _milk.Add(milk.gameObject);
            items[typeof(Milk)] = milk;
            Debug.Log(milk);
        }
    }
}
