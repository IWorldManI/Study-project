using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupPool : PoolerBase<PooledShape>
{
    [SerializeField] private PooledShape _shapePrefab;

    private void Start() 
    {
        InitPool(_shapePrefab); 

        var shape = Get(); 
        Release(shape); 
        
    }
    
    public PooledShape Spawn(Vector3 spawnPosition, int count)
    {
        var shape = Get();
        for (int i = 0; i < count; i++)
        {
            shape.transform.parent = gameObject.transform;
            shape.transform.position = spawnPosition;
            shape.Init(KillShape);
        }
        return shape;
    }

    private void KillShape(PooledShape shape)
    {
        Release(shape);
    }
}
