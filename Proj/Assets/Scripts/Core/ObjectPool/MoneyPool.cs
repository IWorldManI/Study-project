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

    public void Spawn(Vector3 spawnPosition, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var shape = Get();
            shape.transform.parent = gameObject.transform;
            shape.transform.position = spawnPosition;
            shape.Init(KillShape);   
        }
    }

    private void KillShape(PooledShape shape)
    {
        Release(shape);
    }
}
