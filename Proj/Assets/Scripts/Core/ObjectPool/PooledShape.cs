using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledShape : MonoBehaviour
{
    //Test class for debug poll
    
    private Action<PooledShape> _killAction;

    public void Init(Action<PooledShape> killAction)
    {
        _killAction = killAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _killAction(this);
        }
    }
}
