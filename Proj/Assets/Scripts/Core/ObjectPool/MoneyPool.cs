using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyPool : PoolerBase<PooledShape>
{
    [SerializeField] private PooledShape _shapePrefab;

    private void Start() 
    {
        InitPool(_shapePrefab); // Initialize the pool

        var shape = Get(); // Pull from the pool
        Release(shape); // Release back to the pool
        
        
        for (var i = 0; i < 100; i++)
        {
            Spawn();
        }
        
    }

    //for test object pooling
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var shape = Get();
        shape.transform.parent = gameObject.transform;
        shape.transform.position = transform.position + Random.insideUnitSphere * 10;
        shape.Init(KillShape);
    }

    private void KillShape(PooledShape shape)
    {
        Release(shape);
    }
}
