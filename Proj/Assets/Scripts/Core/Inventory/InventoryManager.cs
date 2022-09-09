using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<Type, List<Ingredient>> items = new Dictionary<Type, List<Ingredient>>();
    
    [SerializeField] private List<Ingredient> _ingredient = new List<Ingredient>();

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
            if (item.TryGetComponent<Ingredient>(out Ingredient ingredient))
            {
                _ingredient.Add(ingredient);
                
                if(!items.ContainsKey(typeof(Ingredient)))
                    items.Add(typeof(Ingredient), _ingredient);
                Debug.Log(ingredient);
            }
        }
    }

    public void AddToDictionary(GameObject item)
    {
        if (item.TryGetComponent<Ingredient>(out Ingredient ingredient))
        {
            _ingredient.Add(ingredient);
                
            if(!items.ContainsKey(typeof(Ingredient)))
                items.Add(typeof(Ingredient), _ingredient);
            Debug.Log(ingredient);
        }
    }
}