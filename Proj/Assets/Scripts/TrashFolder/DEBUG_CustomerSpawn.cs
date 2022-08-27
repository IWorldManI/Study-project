using System.Collections;
using System.Collections.Generic;
using Core.Factory;
using UnityEngine;

public class DEBUG_CustomerSpawn : MonoBehaviour
{
    [SerializeField] private CustomerFactory _customerFactory;
    
    void Start()
    {
        _customerFactory = FindObjectOfType<CustomerFactory>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.L)) //performance test 
        {
            var prefab = _customerFactory.CreateNewInstance();
        }
        if (Input.GetKeyDown(KeyCode.K)) //solo spawn test 
        {
            var prefab = _customerFactory.CreateNewInstance();
        }
    }
}
