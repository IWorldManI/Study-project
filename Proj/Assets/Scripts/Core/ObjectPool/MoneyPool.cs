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
        shape.transform.position = spawnPosition;
        shape.Init(KillShape); 
        return shape;
    }

    public void KillShape(PooledShape shape)
    {
        shape.transform.parent = gameObject.transform;
        Release(shape);
    }
}
