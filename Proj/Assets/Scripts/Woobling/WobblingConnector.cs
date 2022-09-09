using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobblingConnector : MonoBehaviour
{
    //Add to list and arranges objects vertically
    
    [SerializeField] public List<GameObject> _itemsInBackpack = new List<GameObject>();
    [SerializeField] public InventoryManager _inventoryManager;
    
    private void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        if(_itemsInBackpack.Count>=0)
            SortItems();
    }

    public void AddItem(Transform item)
    {
        if (item.TryGetComponent<Wobbling>(out Wobbling _wobbling))
        {
            _itemsInBackpack.Add(item.gameObject);
            _wobbling.OnTook = true;
            SetProperties(_wobbling);
            _wobbling.Initialize();
        }
    }

    private void SortItems()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent<Wobbling>(out Wobbling _wobbling))
            {
                if (!_wobbling.OnTook)
                {
                    _itemsInBackpack.Add(_wobbling.gameObject);
                    _wobbling.OnTook = true;
                }
                SetProperties(_wobbling);
            }
        }
    }

    private void SetProperties(Wobbling _wobbling)
    {
        var socket = _itemsInBackpack.Count <= 1 ? transform :  _itemsInBackpack[^2].transform;
        _wobbling.pivot = socket;
        _wobbling.transform.position = new Vector3(0, socket.transform.position.y + 1f, 1);
        _wobbling.stiffness = 399;
        _wobbling.conservation = 0.6f;
    }
}
