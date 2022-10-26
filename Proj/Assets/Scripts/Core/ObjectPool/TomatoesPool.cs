using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoesPool : PoolerBase<PooledShape>
{
    [SerializeField] private PooledShape _shapePrefab;
    
    private void Start() 
    {
        InitPool(_shapePrefab); 

        var shape = Get(); 
        Release(shape); 
        
    }
    //for test object pooling
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            //Spawn(transform.position + Random.insideUnitSphere * 10, 1);
        }
    }

    public PooledShape Spawn(Vector3 spawnPosition)
    {
        var shape = Get();
        shape.transform.parent = gameObject.transform;
        shape.transform.position = spawnPosition;
        shape.Init(KillShape); 
        return shape;
    }

    private void KillShape(PooledShape shape)
    {
        Release(shape);
    }
}
