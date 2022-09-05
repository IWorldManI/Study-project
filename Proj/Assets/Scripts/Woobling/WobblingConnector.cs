using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobblingConnector : MonoBehaviour
{
    //Add to list and arranges objects vertically
    
    [SerializeField] private List<Transform> _itemsInBackpack = new List<Transform>();
    
    void Awake()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent<Wobbling>(out Wobbling _wobbling))
            {
                _itemsInBackpack.Add(_wobbling.transform);
                var socket = _itemsInBackpack.Count <= 1 ? transform : _itemsInBackpack[^2].transform;
                _wobbling.pivot = socket;
                _wobbling.transform.position = new Vector3(0, socket.transform.position.y + 1f, 1);
                Debug.Log(_wobbling.transform.position);
                /*
                    _woobling.transform.localPosition = new Vector3(0, socket.transform.position.y + 1f, 0);
                    Debug.Log(_woobling.transform.localPosition);
                 */
                _wobbling.stiffness = 399;
                _wobbling.conservation = 0.6f;
            }
        }
    }
}
